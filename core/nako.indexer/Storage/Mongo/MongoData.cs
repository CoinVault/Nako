// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoData.cs" company="SoftChains">
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
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using Nako.Client;
    using Nako.Client.Types;
    using Nako.Config;
    using Nako.Extensions;
    using Nako.Operations.Types;
    using Nako.Storage.Mongo.Types;
    using Nako.Storage.Types;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using NBitcoin;
    using NBitcoin.DataEncoders;

    public class MongoData : IStorage
    {
        private readonly ILogger<MongoStorageOperations> log;

        private readonly MongoClient mongoClient;

        private readonly IMongoDatabase mongoDatabase;

        private readonly SyncConnection syncConnection;

        private readonly NakoConfiguration configuration;

        public MongoData(ILogger<MongoStorageOperations> logger, SyncConnection connection, IOptions<NakoConfiguration> nakoConfiguration)
        {
            this.syncConnection = connection;
            this.log = logger;
            this.configuration = nakoConfiguration.Value;
            this.mongoClient = new MongoClient(this.configuration.ConnectionStringActual);
            var dbName = this.configuration.DatabaseNameSubfix ? "Blockchain" + this.configuration.CoinTag : "Blockchain";
            this.mongoDatabase = this.mongoClient.GetDatabase(dbName);
            this.MemoryTransactions = new ConcurrentDictionary<string, NBitcoin.Transaction>();
        }

        public IMongoCollection<MapTransactionAddress> MapTransactionAddress
        {
            get
            {
                return this.mongoDatabase.GetCollection<MapTransactionAddress>("MapTransactionAddress");
            }
        }

        public IMongoCollection<MapTransactionBlock> MapTransactionBlock
        {
            get
            {
                return this.mongoDatabase.GetCollection<MapTransactionBlock>("MapTransactionBlock");
            }
        }

        public IMongoCollection<MapTransaction> MapTransaction
        {
            get
            {
                return this.mongoDatabase.GetCollection<MapTransaction>("MapTransaction");
            }
        }

        public IMongoCollection<MapBlock> MapBlock
        {
            get
            {
                return this.mongoDatabase.GetCollection<MapBlock>("MapBlock");
            }
        }

        public ConcurrentDictionary<string, NBitcoin.Transaction> MemoryTransactions { get; set; }

        public IEnumerable<SyncBlockInfo> BlockGetIncompleteBlocks()
        {
            // note this field is not indexed
            var filter = Builders<MapBlock>.Filter.Eq(info => info.SyncComplete, false);

            return this.MapBlock.Find(filter).ToList().Select(this.Convert);
        }

        public IEnumerable<SyncBlockInfo> BlockGetBlockCount(int count)
        {
            var filter = Builders<MapBlock>.Filter.Exists(info => info.BlockIndex);
            var sort = Builders<MapBlock>.Sort.Descending(info => info.BlockIndex);

            return this.MapBlock.Find(filter).Sort(sort).Limit(count).ToList().Select(this.Convert);
        }

        public IEnumerable<SyncBlockInfo> BlockGetCompleteBlockCount(int count)
        {
            var blocks = this.BlockGetBlockCount(2).ToList();
            return blocks.Where(b => b.SyncComplete);
        }

        public SyncBlockInfo BlockGetByIndex(long blockIndex)
        {
            var filter = Builders<MapBlock>.Filter.Eq(info => info.BlockIndex, blockIndex);

            return this.MapBlock.Find(filter).ToList().Select(this.Convert).FirstOrDefault();
        }

        public SyncBlockInfo BlockGetByHash(string blockHash)
        {
            var filter = Builders<MapBlock>.Filter.Eq(info => info.BlockHash, blockHash);

            return this.MapBlock.Find(filter).ToList().Select(this.Convert).FirstOrDefault();
        }

        public void InsertBlock(MapBlock info)
        {
            this.MapBlock.InsertOne(info);
        }

        public SyncRawTransaction TransactionGetByHash(string trxHash)
        {
            var filter = Builders<MapTransaction>.Filter.Eq(info => info.TransactionId, trxHash);

            return this.MapTransaction.Find(filter).ToList().Select(t => new SyncRawTransaction { TransactionHash = trxHash, RawTransaction = t.RawTransaction }).FirstOrDefault();
        }

        public void InsertTransaction(MapTransaction info)
        {
            this.MapTransaction.InsertOne(info);
        }

        public void CompleteBlock(string blockHash)
        {
            var filter = Builders<MapBlock>.Filter.Eq(blockInfo => blockInfo.BlockHash, blockHash);
            var update = Builders<MapBlock>.Update.Set(blockInfo => blockInfo.SyncComplete, true);
            this.MapBlock.UpdateOne(filter, update);
        }

        public void MarkOutput(string transaction, int index, string spendingTransactionId)
        {
            var filter = Builders<MapTransactionAddress>.Filter.Eq(addr => addr.Id, string.Format("{0}-{1}", transaction, index));
            var update = Builders<MapTransactionAddress>.Update.Set(blockInfo => blockInfo.SpendingTransactionId, spendingTransactionId);
            this.MapTransactionAddress.UpdateOne(filter, update);
        }

        public string GetSpendingTransaction(string transaction, int index)
        {
            var filter = Builders<MapTransactionAddress>.Filter.Eq(addr => addr.Id, string.Format("{0}-{1}", transaction, index));

            return this.MapTransactionAddress.Find(filter).ToList().Select(t => t.SpendingTransactionId).FirstOrDefault();
        }

        public SyncTransactionInfo BlockTransactionGet(string transactionId)
        {
            var filter = Builders<MapTransactionBlock>.Filter.Eq(info => info.TransactionId, transactionId);

            var trx = this.MapTransactionBlock.Find(filter).FirstOrDefault();
            if (trx == null)
            {
                return null;
            }

            var current = this.BlockGetBlockCount(1).First();

            var blk = this.BlockGetByIndex(trx.BlockIndex);
                
            return new SyncTransactionInfo
            {
                BlockIndex = trx.BlockIndex, 
                BlockHash = blk.BlockHash, 
                Timestamp = blk.BlockTime, 
                TransactionHash = trx.TransactionId, 
                Confirmations = current.BlockIndex - trx.BlockIndex
            };
        }

        public IEnumerable<SyncTransactionInfo> BlockTransactionGetByBlock(string blockHash)
        {
            var blk = this.BlockGetByHash(blockHash);
            var current = this.BlockGetBlockCount(1).First();

            var filter = Builders<MapTransactionBlock>.Filter.Eq(info => info.BlockIndex, blk.BlockIndex);
            var trxs = this.MapTransactionBlock.Find(filter).ToList();

            return trxs.Select(s => new SyncTransactionInfo
                                        {
                                            BlockIndex = s.BlockIndex, 
                                            BlockHash = blk.BlockHash, 
                                            Timestamp = blk.BlockTime, 
                                            TransactionHash = s.TransactionId, 
                                            Confirmations = current.BlockIndex - s.BlockIndex
                                        });
        }

        public IEnumerable<SyncTransactionInfo> BlockTransactionGetByBlockIndex(long blockIndex)
        {
            var blk = this.BlockGetByIndex(blockIndex);
            var current = this.BlockGetBlockCount(1).First();

            var filter = Builders<MapTransactionBlock>.Filter.Eq(info => info.BlockIndex, blk.BlockIndex);
            var trxs = this.MapTransactionBlock.Find(filter).ToList();

            return trxs.Select(s => new SyncTransactionInfo
            {
                BlockIndex = s.BlockIndex, 
                BlockHash = blk.BlockHash, 
                Timestamp = blk.BlockTime, 
                TransactionHash = s.TransactionId, 
                Confirmations = current.BlockIndex - s.BlockIndex
            });
        }

        public SyncTransactionItemOutput TransactionsGet(string transactionId, int index, SyncTransactionIndexType indexType)
        {
            throw new NotImplementedException();
        }

        public SyncTransactionItems TransactionItemsGet(string transactionId)
        {
            NBitcoin.Transaction transaction;

            // Try to find the trx in disk
            var rawtrx = this.TransactionGetByHash(transactionId);
            if(rawtrx == null)
            {
                var client = CryptoClientFactory.Create(this.syncConnection.ServerDomain, this.syncConnection.RpcAccessPort, this.syncConnection.User, this.syncConnection.Password, this.syncConnection.Secure);

                var res = client.GetRawTransactionAsync(transactionId, 0).Result;

                transaction = this.syncConnection.Network.Consensus.ConsensusFactory.CreateTransaction(res.Hex);
                transaction.PrecomputeHash(false, true);
            }
            else
            {
                transaction = this.syncConnection.Network.Consensus.ConsensusFactory.CreateTransaction(rawtrx.RawTransaction);
                transaction.PrecomputeHash(false, true);
            }

            var ret = new SyncTransactionItems
                       {
                           RBF = transaction.RBF,
                           LockTime = transaction.LockTime.ToString(),
                           Version = transaction.Version,
                           IsCoinbase = transaction.IsCoinBase,
                           IsCoinstake = this.syncConnection.Network.Consensus.IsProofOfStake && transaction.IsCoinStake,
                           Inputs = transaction.Inputs.Select(v => new SyncTransactionItemInput
                           {
                               PreviousTransactionHash = v.PrevOut.Hash.ToString(),
                               PreviousIndex = (int)v.PrevOut.N,
                               WitScript = v.WitScript.ToScript().ToHex(),
                               ScriptSig = v.ScriptSig.ToHex(),
                               SequenceLock = v.Sequence.ToString(),
                           })
                           .ToList(),
                           Outputs = transaction.Outputs.Select((output, index) => new SyncTransactionItemOutput
                           {
                               Address = ScriptToAddressParser.GetAddress(this.syncConnection.Network, output.ScriptPubKey)?.FirstOrDefault(),
                               Index = index,
                               Value = (long)output.Value,
                               OutputType = StandardScripts.GetTemplateFromScriptPubKey(output.ScriptPubKey)?.Type.ToString(),
                               ScriptPubKey = output.ScriptPubKey.ToHex()
                           })
                           .ToList()
                       };


            // try to fetch spent outputs
            foreach (var output in ret.Outputs)
            {
                output.SpentInTransaction = this.GetSpendingTransaction(transactionId, output.Index);
            }

            return ret;
        }

        public SyncTransactionAddressBalance AddressGetBalance(string address, long confirmations)
        {
            var current = this.BlockGetBlockCount(1).First();

            var addrs = this.SelectAddressWithPool(current, address, false).ToList();
            return this.CreateAddresBalance(confirmations, addrs, false);
        }

        public SyncTransactionAddressBalance AddressGetBalanceUtxo(string address, long confirmations)
        {
            var current = this.BlockGetBlockCount(1).First();

            var addrs = this.SelectAddressWithPool(current, address, true).ToList();
            return this.CreateAddresBalance(confirmations, addrs, true);
        }

        public void DeleteBlock(string blockHash)
        {
            var block = this.BlockGetByHash(blockHash);

            // delete the outputs
            var addrFilter = Builders<MapTransactionAddress>.Filter.Eq(addr => addr.BlockIndex, block.BlockIndex);
            this.MapTransactionAddress.DeleteMany(addrFilter);

            // delete the transaction
            var transactionFilter = Builders<MapTransactionBlock>.Filter.Eq(info => info.BlockIndex, block.BlockIndex);
            this.MapTransactionBlock.DeleteMany(transactionFilter);

            // delete the block itself.
            var blockFilter = Builders<MapBlock>.Filter.Eq(info => info.BlockHash, blockHash);
            this.MapBlock.DeleteOne(blockFilter);
        }

        public IEnumerable<NBitcoin.Transaction> GetMemoryTransactions()
        {
            return this.MemoryTransactions.Values;
        }

        #region private

        private SyncTransactionAddressBalance CreateAddresBalance(long confirmations, List<SyncTransactionAddressItem> addrs, bool availableOnly)
        {
            var all = addrs.Where(s => s.Confirmations >= confirmations).Sum(s => s.Value);
            var confirming = addrs.Where(s => s.Confirmations < confirmations).Sum(s => s.Value);
            var used = addrs.Where(s => s.SpendingTransactionHash != null).Sum(s => s.Value);
            var available = all - used;

            return new SyncTransactionAddressBalance
                       {
                           Available = available, 
                           Received = availableOnly ? default(long?) : all, 
                           Sent = availableOnly ? default(long?) : used, 
                           Unconfirmed = confirming, 
                           Items = addrs
                       };
        }

        private SyncBlockInfo Convert(MapBlock block)
        {
            return new SyncBlockInfo
            {
                BlockIndex = block.BlockIndex,
                BlockSize = block.BlockSize,
                BlockHash = block.BlockHash,
                BlockTime = block.BlockTime,
                NextBlockHash = block.NextBlockHash,
                PreviousBlockHash = block.PreviousBlockHash,
                TransactionCount = block.TransactionCount,
                Nonce = block.Nonce,
                ChainWork = block.ChainWork,
                Difficulty = block.Difficulty,
                Merkleroot = block.Merkleroot,
                PosModifierv2 = block.PosModifierv2,
                PosHashProof = block.PosHashProof,
                PosFlags = block.PosFlags,
                PosChainTrust = block.PosChainTrust,
                PosBlockTrust = block.PosBlockTrust,
                PosBlockSignature = block.PosBlockSignature,
                Confirmations = block.Confirmations,
                Bits = block.Bits,
                Version = block.Version,
                SyncComplete = block.SyncComplete
            };
        }

        private IEnumerable<SyncTransactionAddressItem> SelectAddressWithPool(SyncBlockInfo current, string address, bool availableOnly)
        {
            var builder = Builders<MapTransactionAddress>.Filter;
            var addressFiler = new List<string> {address};
            var filter = builder.AnyIn(transactionAddress => transactionAddress.Addresses, addressFiler);

            if (availableOnly)
            {
                // we only want spendable transactions    
                filter = filter & builder.Eq(info => info.SpendingTransactionId, null);
            }

            var stoper1 = Stopwatch.Start();

            var sort = Builders<MapTransactionAddress>.Sort.Descending(info => info.BlockIndex);

            var addrs = this.MapTransactionAddress.Find(filter).Sort(sort).ToList();

            stoper1.Stop();

            this.log.LogInformation($"Select: Seconds = {stoper1.Elapsed.TotalSeconds} - UnspentOnly = {availableOnly} - Addr = {address} - Items = {addrs.Count()}");

            // this creates a copy of the collection (to avoid thread issues)
            var pool = this.MemoryTransactions.Values;

            if (pool.Any())
            {
                // mark trx in output as spent if they exist in the pool
                var addrsupdate = addrs;
                this.GetPoolOutputs(pool).ForEach(f =>
                {
                    var adr = addrsupdate.FirstOrDefault(a => a.TransactionId == f.Item1.PrevOut.Hash.ToString() && a.Index == f.Item1.PrevOut.N);
                    if (adr != null)
                    {
                        adr.SpendingTransactionId = f.Item2;
                    }
                });

                // if only spendable transactions are to be returned we need to remove 
                // any that have been marked as spent by a transaction in the pool
                if (availableOnly)
                {
                    addrs = addrs.Where(d => d.SpendingTransactionId == null).ToList();
                }

                // add all pool transactions to main output
                var paddr = this.PoolToMapTransactionAddress(pool, address).ToList();
                addrs = addrs.OrderByDescending(s => s.BlockIndex).Concat(paddr).ToList();
            }

            // map to return type and calculate confirmations
            return addrs.Select(s => new SyncTransactionAddressItem
            {
                Address = address, 
                Index = s.Index, 
                TransactionHash = s.TransactionId, 
                BlockIndex = s.BlockIndex == -1 ? default(long?) : s.BlockIndex, 
                Value = s.Value, 
                Confirmations = s.BlockIndex == -1 ? 0 : current.BlockIndex - s.BlockIndex + 1, 
                SpendingTransactionHash = s.SpendingTransactionId, 
                CoinBase = s.CoinBase,
                CoinStake = s.CoinStake,
                ScriptHex = new Script(Encoders.Hex.DecodeData(s.ScriptHex)).ToString(),
                Type = StandardScripts.GetTemplateFromScriptPubKey(new Script(Encoders.Hex.DecodeData(s.ScriptHex)))?.Type.ToString(),
                Time = s.BlockIndex == -1 ? UnixUtils.DateToUnixTimestamp(DateTime.UtcNow) : current.BlockTime
            });
        }

        private IEnumerable<Tuple<NBitcoin.TxIn, string>> GetPoolOutputs(IEnumerable<NBitcoin.Transaction> pool)
        {
            return pool.SelectMany(s => s.Inputs.Select(v => new Tuple<NBitcoin.TxIn, string>(v, s.GetHash().ToString())));
        }

        private IEnumerable<MapTransactionAddress> PoolToMapTransactionAddress(IEnumerable<NBitcoin.Transaction> pool, string address)
        {
            foreach (var transaction in pool)
            {
                var rawTransaction = transaction;

                var index = 0;
                foreach (var output in rawTransaction.Outputs)
                {
                    var addressIndex = ScriptToAddressParser.GetAddress(this.syncConnection.Network, output.ScriptPubKey);

                    if (address == addressIndex.FirstOrDefault())
                        continue;

                    var id = rawTransaction.GetHash().ToString();

                    yield return new MapTransactionAddress
                    {
                        Id = string.Format("{0}-{1}", id, index),
                        TransactionId = id,
                        Value = output.Value,
                        Index = index++,
                        Addresses = new List<string> {address},
                        ScriptHex = output.ScriptPubKey.ToHex(),
                        BlockIndex = -1,
                        CoinBase = rawTransaction.IsCoinBase,
                        CoinStake = rawTransaction.IsCoinStake,
                    };
                }
            }
        }

        public class SelectBalanceResult
        {
            public double TotalAmount { get; set; }

            public int Count { get; set; }

            public SelectStats id { get; set; }

            public class SelectStats
            {
                public bool Spent { get; set; }
                public bool Confirmed { get; set; }
            }
        }

        #endregion
    }
}
