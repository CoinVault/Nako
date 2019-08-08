// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncOperations.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Nako.Crypto;

namespace Nako.Sync
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Nako.Client;
    using Nako.Client.Types;
    using Nako.Config;
    using Nako.Extensions;
    using Nako.Operations;
    using Nako.Operations.Types;
    using Nako.Storage;
    using NBitcoin;
    using NBitcoin.DataEncoders;

    /// <summary>
    /// The CoinOperations interface.
    /// </summary>
    public class SyncOperations : ISyncOperations
    {
        private readonly IStorage storage;

        private readonly ILogger<SyncOperations> log;

        private readonly NakoConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncOperations"/> class.
        /// </summary>
        public SyncOperations(IStorage storage, ILogger<SyncOperations> logger, IOptions<NakoConfiguration> configuration)
        {
            this.configuration = configuration.Value;
            this.log = logger;
            this.storage = storage;

            // Register the cold staking template.
            StandardScripts.RegisterStandardScriptTemplate(ColdStakingScriptTemplate.Instance);
        }

        public SyncBlockOperation FindBlock(SyncConnection connection, SyncingBlocks container)
        {
            return this.FindBlockInternal(connection, container);
        }

        public SyncPoolTransactions FindPoolTransactions(SyncConnection connection, SyncingBlocks container)
        {
            return this.FindPoolInternal(connection, container);
        }

        public SyncBlockTransactionsOperation SyncPool(SyncConnection connection, SyncPoolTransactions poolTransactions)
        {
            return this.SyncPoolInternal(connection, poolTransactions);
        }

        public SyncBlockTransactionsOperation SyncBlock(SyncConnection connection, BlockInfo block)
        {
            return this.SyncBlockInternal(connection, block);
        }

        public async Task CheckBlockReorganization(SyncConnection connection)
        {
            while (true)
            {
                var block = this.storage.BlockGetBlockCount(1).FirstOrDefault();

                if (block == null)
                {
                    break;
                }

                var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);
                var currentHash = await client.GetblockHashAsync(block.BlockIndex);
                if (currentHash == block.BlockHash)
                {
                    break;
                }

                this.log.LogInformation($"SyncOperations: Deleting block {block.BlockIndex}");

                this.storage.DeleteBlock(block.BlockHash);
            }
        }

        private SyncBlockOperation GetNextBlockToSync(BitcoinClient client, SyncConnection connection, long lastCryptoBlockIndex, SyncingBlocks syncingBlocks)
        {
            if (syncingBlocks.LastBlock == null)
            {
                // because inserting blocks is sequential we'll use the indexed 'height' filed to check if the last block is incomplete.
                var incomplete = this.storage.BlockGetBlockCount(6).Where(b => !b.SyncComplete).ToList(); ////this.storage.BlockGetIncompleteBlocks().ToList();

                var incompleteToSync = incomplete.OrderBy(o => o.BlockIndex).FirstOrDefault(f => !syncingBlocks.CurrentSyncing.ContainsKey(f.BlockHash));

                if (incompleteToSync != null)
                {
                    var incompleteBlock = client.GetBlock(incompleteToSync.BlockHash);

                    return new SyncBlockOperation { BlockInfo = incompleteBlock, IncompleteBlock = true, LastCryptoBlockIndex = lastCryptoBlockIndex };
                }

                string blockHashsToSync;

                var blokcs = this.storage.BlockGetBlockCount(1).ToList();

                if (blokcs.Any())
                {
                    var lastBlockIndex = blokcs.First().BlockIndex;

                    if (lastBlockIndex == lastCryptoBlockIndex)
                    {
                        // No new blocks.
                        return default(SyncBlockOperation);
                    }

                    blockHashsToSync = client.GetblockHash(lastBlockIndex + 1);
                }
                else
                {
                    // No blocks in store start from zero configured block index.
                    blockHashsToSync = client.GetblockHash(connection.StartBlockIndex);
                }

                var nextNewBlock = client.GetBlock(blockHashsToSync);

                syncingBlocks.LastBlock = nextNewBlock;

                return new SyncBlockOperation { BlockInfo = nextNewBlock, LastCryptoBlockIndex = lastCryptoBlockIndex };
            }

            if (syncingBlocks.LastBlock.Height == lastCryptoBlockIndex)
            {
                // No new blocks.
                return default(SyncBlockOperation);
            }

            var nextHash = client.GetblockHash(syncingBlocks.LastBlock.Height + 1);

            var nextBlock = client.GetBlock(nextHash);

            syncingBlocks.LastBlock = nextBlock;

            return new SyncBlockOperation { BlockInfo = nextBlock, LastCryptoBlockIndex = lastCryptoBlockIndex };
        }

        private SyncBlockOperation FindBlockInternal(SyncConnection connection, SyncingBlocks syncingBlocks)
        {
            var stoper = Stopwatch.Start();

            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            var lastCryptoBlockIndex = client.GetBlockCount();

            var blockToSync = this.GetNextBlockToSync(client, connection, lastCryptoBlockIndex, syncingBlocks);

            if (blockToSync != null && blockToSync.BlockInfo != null)
            {
                syncingBlocks.CurrentSyncing.TryAdd(blockToSync.BlockInfo.Hash, blockToSync.BlockInfo);
            }
           
            stoper.Stop();

            return blockToSync;
        }

        private SyncPoolTransactions FindPoolInternal(SyncConnection connection, SyncingBlocks syncingBlocks)
        {
            var stoper = Stopwatch.Start();

            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            if (client.GetBlockchainInfo().Result.IsInitialBlockDownload)
            {
                return new SyncPoolTransactions { Transactions = new List<string>() };
            }

            var memPool = client.GetRawMemPool();

            var currentMemoryPool = new HashSet<string>(memPool);
            var currentTable = new HashSet<string>(syncingBlocks.CurrentPoolSyncing);

            var newTransactions = currentMemoryPool.Except(currentTable).ToList();
            var deleteTransaction = currentTable.Except(currentMemoryPool).ToList();

            //var newTransactionsLimited = newTransactions.Count() < 1000 ? newTransactions : newTransactions.Take(1000).ToList();

            syncingBlocks.CurrentPoolSyncing.AddRange(newTransactions);
            deleteTransaction.ForEach(t => syncingBlocks.CurrentPoolSyncing.Remove(t));

            stoper.Stop();

            this.log.LogDebug($"SyncPool: Seconds = {stoper.Elapsed.TotalSeconds} - New Transactions = {newTransactions.Count()}");

            return new SyncPoolTransactions { Transactions = newTransactions };
        }

        private class tcalc
        {
            public string item;
            public DecodedRawTransaction result;
        }

        private SyncBlockTransactionsOperation SyncBlockTransactions(BitcoinClient client, SyncConnection connection, IEnumerable<string> transactionsToSync, bool throwIfNotFound)
        {
            var stoper = new System.Diagnostics.Stopwatch();
            stoper.Start();

            var itemList = transactionsToSync.Select(t => new tcalc { item = t }).ToList();

            var options = new ParallelOptions { MaxDegreeOfParallelism = this.configuration.ParallelRequestsToTransactionRpc };
            Parallel.ForEach(itemList, options, (item) =>
            {
                try
                {
                    item.result = client.GetRawTransaction(item.item, 0);
                }
                catch (BitcoinClientException bce)
                {
                    if (!throwIfNotFound && bce.IsTransactionNotFound())
                    {
                        //// the transaction was not found in the client, 
                        //// if this is a pool sync we assume the transaction was initially found in the pool and became invalid.
                        return;
                    }

                    throw;
                }
            });

            var transactions = itemList.Select(s =>
            {
                var trx = connection.Network.Consensus.ConsensusFactory.CreateTransaction(s.result.Hex);
                trx.PrecomputeHash(false, true);
                return trx;
            });

            return new SyncBlockTransactionsOperation { Transactions = transactions.ToList() };
        }


        private SyncBlockTransactionsOperation SyncPoolInternal(SyncConnection connection, SyncPoolTransactions poolTransactions)
        {
            var watch = Stopwatch.Start();

            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            var returnBlock = this.SyncBlockTransactions(client, connection, poolTransactions.Transactions, false);

            watch.Stop();

            var transactionCount = returnBlock.Transactions.Count();
            var totalSeconds = watch.Elapsed.TotalSeconds;

            this.log.LogDebug($"SyncPool: Seconds = {watch.Elapsed.TotalSeconds} - Transactions = {transactionCount}");

            return returnBlock;
        }

        private SyncBlockTransactionsOperation SyncBlockInternal(SyncConnection connection, BlockInfo block)
        {
            var watch = Stopwatch.Start();

            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            var hex = client.GetBlockHex(block.Hash);

            Block blockItem = Block.Parse(hex, connection.Network);

            foreach (var blockItemTransaction in blockItem.Transactions)
            {
                blockItemTransaction.PrecomputeHash(false, true);
            }

            //var blockItem = Block.Load(Encoders.Hex.DecodeData(hex), consensusFactory);
            var returnBlock = new SyncBlockTransactionsOperation {BlockInfo = block, Transactions = blockItem.Transactions };  //this.SyncBlockTransactions(client, connection, block.Transactions, true);

            watch.Stop();

            return returnBlock;
        }
    }
}
