// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoStorageOperations.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Nako.Crypto;

namespace Nako.Storage.Mongo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using Nako.Client.Types;
    using Nako.Config;
    using Nako.Extensions;
    using Nako.Operations;
    using Nako.Operations.Types;
    using Nako.Storage.Mongo.Types;
    using Nako.Storage.Types;
    using Nako.Sync;
    using NBitcoin;

    /// <summary>
    /// Mongo storage operations.
    /// </summary>
    public class MongoStorageOperations : IStorageOperations
    {
        private readonly IStorage storage;

        private readonly ILogger<MongoStorageOperations> log;
        private readonly SyncConnection syncConnection;

        private readonly NakoConfiguration configuration;

        private readonly MongoData data;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoStorageOperations"/> class.
        /// </summary>
        public MongoStorageOperations(IStorage storage, ILogger<MongoStorageOperations> logger, IOptions<NakoConfiguration> configuration, SyncConnection syncConnection)
        {
            this.data = (MongoData)storage;
            this.configuration = configuration.Value;
            this.log = logger;
            this.syncConnection = syncConnection;
            this.storage = storage;
        }

        public void ValidateBlock(SyncBlockTransactionsOperation item)
        {
            if (item.BlockInfo != null)
            {
                var lastBlock = this.storage.BlockGetBlockCount(1).FirstOrDefault();

                if (lastBlock != null)
                {
                    if (lastBlock.BlockHash == item.BlockInfo.Hash)
                    {
                        if (lastBlock.SyncComplete)
                        {
                            throw new InvalidOperationException("This should never happen.");
                        }
                    }
                    else
                    {
                        if (item.BlockInfo.PreviousBlockHash != lastBlock.BlockHash)
                        {
                            this.InvalidBlockFound(lastBlock, item);
                            return;
                        }

                        this.CreateBlock(item.BlockInfo);
                    }
                }
                else
                {
                    this.CreateBlock(item.BlockInfo);
                }
            }
        }

        public InsertStats InsertTransactions(SyncBlockTransactionsOperation item)
        {
            var stats = new InsertStats { Items = new List<MapTransactionAddress>() };

            if (item.BlockInfo != null)
            {
                // remove all transactions from the memory pool
                item.Transactions.ForEach(t =>
                    {
                        NBitcoin.Transaction outer;
                        this.data.MemoryTransactions.TryRemove(t.GetHash().ToString(), out outer);
                    });


                var items = new List<Transaction>(item.Transactions);

                // insert all the mappings of transactions to a block
                try
                {
                    if (item.BlockInfo != null)
                    {
                        var inserts = items.Select(s => new MapTransactionBlock { BlockIndex = item.BlockInfo.Height, TransactionId = s.GetHash().ToString() }).ToList();
                        stats.Transactions += inserts.Count;
                        this.data.MapTransactionBlock.InsertMany(inserts, new InsertManyOptions { IsOrdered = false });
                    }
                }
                catch (MongoBulkWriteException mbwex)
                {
                    if (mbwex.WriteErrors.Any(e => e.Category != ServerErrorCategory.DuplicateKey))
                    {
                        throw;
                    }
                }

                // insert inputs and add to the list for later to use on the notification task.
                var inputs = this.CreateInputs(item.BlockInfo.Height, items).ToList();
                var outputs = this.CreateOutputs(items, item.BlockInfo.Height).ToList();
                inputs.AddRange(outputs);

                try
                {
                    var ops = new Dictionary<string, WriteModel<MapTransactionAddress>>();
                    var writeOptions = new BulkWriteOptions() { IsOrdered = false };

                    foreach (var mapTransactionAddress in inputs)
                    {
                        if(mapTransactionAddress.SpendingTransactionId == null)
                        {
                            ops.Add(mapTransactionAddress.Id, new InsertOneModel<MapTransactionAddress>(mapTransactionAddress));
                        }
                        else
                        {
                            if (ops.TryGetValue(mapTransactionAddress.Id, out WriteModel<MapTransactionAddress> mta))
                            {
                                // in case a utxo is spent in the same block
                                // we just modify the inserted item directly

                                var imta = mta as InsertOneModel<MapTransactionAddress>;
                                imta.Document.SpendingTransactionId = mapTransactionAddress.SpendingTransactionId;
                                imta.Document.SpendingBlockIndex = mapTransactionAddress.SpendingBlockIndex;
                            }
                            else
                            {
                                var filter = Builders<MapTransactionAddress>.Filter.Eq(addr => addr.Id, mapTransactionAddress.Id);

                                var update = Builders<MapTransactionAddress>.Update
                                    .Set(blockInfo => blockInfo.SpendingTransactionId, mapTransactionAddress.SpendingTransactionId)
                                    .Set(blockInfo => blockInfo.SpendingBlockIndex, mapTransactionAddress.SpendingBlockIndex);

                                ops.Add(mapTransactionAddress.Id, new UpdateOneModel<MapTransactionAddress>(filter, update));
                            }
                        }
                    }

                    if (ops.Any())
                    {
                        stats.Items.AddRange(inputs);
                        stats.InputsOutputs += ops.Count;
                        this.data.MapTransactionAddress.BulkWrite(ops.Values, writeOptions);
                    }
                }
                catch (MongoBulkWriteException mbwex)
                {
                    if (mbwex.WriteErrors.Any(e => e.Category != ServerErrorCategory.DuplicateKey))//.Message.Contains("E11000 duplicate key error collection"))
                    {
                        throw;
                    }
                }

                // If insert trx supported then push trx in batches.
                if (this.configuration.StoreRawTransactions)
                {
                    try
                    {
                        var inserts = items.Select(t => new MapTransaction { TransactionId = t.GetHash().ToString(), RawTransaction = t.ToBytes(syncConnection.Network.Consensus.ConsensusFactory) }).ToList();
                        stats.RawTransactions = inserts.Count;
                        this.data.MapTransaction.InsertMany(inserts, new InsertManyOptions { IsOrdered = false });
                    }
                    catch (MongoBulkWriteException mbwex)
                    {
                        if (mbwex.WriteErrors.Any(e => e.Category != ServerErrorCategory.DuplicateKey))//.Message.Contains("E11000 duplicate key error collection"))
                        {
                            throw;
                        }
                    }
                }

                // mark the block as synced.
                this.CompleteBlock(item.BlockInfo);
            }
            else
            {
                // memory transaction push in to the pool.
                item.Transactions.ForEach(t =>
                {
                    this.data.MemoryTransactions.TryAdd(t.GetHash().ToString(), t);
                });

                stats.Transactions = this.data.MemoryTransactions.Count();

                // todo: for accuracy - remove transactions from the mongo memory pool that are not anymore in the syncing pool
                // remove all transactions from the memory pool
                // this can be done using the SyncingBlocks objects - see method SyncOperations.FindPoolInternal()

                // add to the list for later to use on the notification task.
                var inputs = this.CreateInputs(-1, item.Transactions).ToList();
                stats.Items.AddRange(inputs);
            }

            return stats;
        }
        

        private void CompleteBlock(BlockInfo block)
        {
            this.data.CompleteBlock(block.Hash);
        }

        private void CreateBlock(BlockInfo block)
        {
            var blockInfo = new MapBlock
            {
                BlockIndex = block.Height, 
                BlockHash = block.Hash, 
                BlockSize = block.Size, 
                BlockTime = block.Time, 
                NextBlockHash = block.NextBlockHash, 
                PreviousBlockHash = block.PreviousBlockHash, 
                TransactionCount = block.Transactions.Count(), 
                Bits = block.Bits,
                Confirmations = block.Confirmations,
                Merkleroot = block.Merkleroot,
                Nonce = block.Nonce,
                ChainWork = block.ChainWork,
                Difficulty = block.Difficulty,
                PosBlockSignature = block.PosBlockSignature,
                PosBlockTrust = block.PosBlockTrust,
                PosChainTrust = block.PosChainTrust,
                PosFlags = block.PosFlags,
                PosHashProof = block.PosHashProof,
                PosModifierv2 = block.PosModifierv2,
                Version = block.Version,
                SyncComplete = false
            };

            this.data.InsertBlock(blockInfo);
        }

        private IEnumerable<T> GetBatch<T>(int maxItems, Queue<T> queue)
        {
            //var total = 0;
            var items = new List<T>();

            // todo: optimize this
            var aggregate = Nako.Extensions.Extensions.TakeAndRemove(queue, maxItems).ToList();
            items.AddRange(aggregate);

            return items;
        }

        private void InvalidBlockFound(SyncBlockInfo lastBlock, SyncBlockTransactionsOperation item)
        {
            // Re-org happened.
            throw new SyncRestartException();
        }

        private IEnumerable<SyncTransactionInfo> CreateTransactions(BlockInfo block, IEnumerable<DecodedRawTransaction> transactions)
        {
            var trxInfps = transactions.Select(trx => new SyncTransactionInfo
            {
                TransactionHash = trx.TxId, 
                Timestamp = block == null ? UnixUtils.DateToUnixTimestamp(DateTime.UtcNow) : block.Time
            });

            return trxInfps;
        }

        private IEnumerable<MapTransactionAddress> CreateInputs(long blockIndex, IEnumerable<NBitcoin.Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                var rawTransaction = transaction;

                var id = rawTransaction.GetHash().ToString();

                for (int index = 0; index < rawTransaction.Outputs.Count; index++)
                {
                    var output = rawTransaction.Outputs[index];

                    var address = ScriptToAddressParser.GetAddress(this.syncConnection.Network, output.ScriptPubKey);

                    if(address == null)
                        continue;

                    yield return new MapTransactionAddress
                    {
                        Id = string.Format("{0}-{1}", id, index),
                        TransactionId = id,
                        Value = output.Value,
                        Index = index,
                        Addresses = address.ToList(), 
                        ScriptHex = output.ScriptPubKey.ToHex(),
                        BlockIndex = blockIndex,
                        CoinBase = rawTransaction.IsCoinBase,
                        CoinStake = this.syncConnection.Network.Consensus.IsProofOfStake && rawTransaction.IsCoinStake,
                    };
                }
            }
        }

        private IEnumerable<MapTransactionAddress> CreateOutputs(IEnumerable<NBitcoin.Transaction> transactions, long blockIndex)
        {
            foreach (var transaction in transactions)
            {
                if(transaction.IsCoinBase)
                    continue;

                foreach (var input in transaction.Inputs)
                {
                    yield return new MapTransactionAddress
                    {
                        Id = string.Format("{0}-{1}", input.PrevOut.Hash, input.PrevOut.N),
                        SpendingTransactionId = transaction.GetHash().ToString(),
                        SpendingBlockIndex = blockIndex,
                    };

                }
            }
        }
    }
}
