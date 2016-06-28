// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncOperations.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Sync
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Nako.Client;
    using Nako.Client.Types;
    using Nako.Config;
    using Nako.Extensions;
    using Nako.Operations;
    using Nako.Operations.Types;
    using Nako.Storage;

    #endregion

    /// <summary>
    /// The CoinOperations interface.
    /// </summary>
    public class SyncOperations : ISyncOperations
    {
        private readonly IStorage storage;

        private readonly Tracer tracer;

        private readonly NakoConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncOperations"/> class.
        /// </summary>
        public SyncOperations(IStorage storage, Tracer tracer, NakoConfiguration nakoConfiguration)
        {
            this.configuration = nakoConfiguration;
            this.tracer = tracer;
            this.storage = storage;
        }

        #region Public Methods and Operators

        public Task<SyncBlockOperation> FindBlock(SyncConnection connection, SyncingBlocks container)
        {
            return this.FindBlockInternal(connection, container);
        }

        public Task<SyncPoolTransactions> FindPoolTransactions(SyncConnection connection, SyncingBlocks container)
        {
            return this.FindPoolInternal(connection, container);
        }

        public Task<SyncBlockTransactionsOperation> SyncPool(SyncConnection connection, SyncPoolTransactions poolTransactions)
        {
            return this.SyncPoolInternal(connection, poolTransactions);
        }

        public Task<SyncBlockTransactionsOperation> SyncBlock(SyncConnection connection, BlockInfo block)
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

                this.tracer.Trace("SyncOperations", string.Format("Deleting block {0}", block.BlockIndex));

                this.storage.DeleteBlock(block.BlockHash);
            }
        }

        #endregion

        #region Methods

        private Task<SyncBlockOperation> GetNextBlockToSync(BitcoinClient client, SyncConnection connection, long lastCryptoBlockIndex, SyncingBlocks syncingBlocks)
        {
            if (syncingBlocks.LastBlock == null)
            {
                // because inserting blocks is sequential we'll use the indexed 'height' filed to check if the last block is incomplete.
                var incomplete = this.storage.BlockGetBlockCount(6).Where(b => !b.SyncComplete).ToList(); ////this.storage.BlockGetIncompleteBlocks().ToList();

                var incompleteToSync = incomplete.OrderBy(o => o.BlockIndex).FirstOrDefault(f => !syncingBlocks.CurrentSyncing.ContainsKey(f.BlockHash));

                if (incompleteToSync != null)
                {
                    var incompleteBlock = client.GetBlock(incompleteToSync.BlockHash).Result;

                    return Task.FromResult(new SyncBlockOperation { BlockInfo = incompleteBlock, IncompleteBlock = true, LastCryptoBlockIndex = lastCryptoBlockIndex });
                }

                string blockHashsToSync;

                var blokcs = this.storage.BlockGetBlockCount(1).ToList();

                if (blokcs.Any())
                {
                    var lastBlockIndex = blokcs.First().BlockIndex;

                    if (lastBlockIndex == lastCryptoBlockIndex)
                    {
                        // No new blocks.
                        return Task.FromResult(default(SyncBlockOperation));
                    }

                    blockHashsToSync = client.GetblockHash(lastBlockIndex + 1).Result;
                }
                else
                {
                    // No blocks in store start from zero configured block index.
                    blockHashsToSync = client.GetblockHash(connection.StartBlockIndex).Result;
                }

                var nextNewBlock = client.GetBlock(blockHashsToSync).Result;

                syncingBlocks.LastBlock = nextNewBlock;

                return Task.FromResult(new SyncBlockOperation { BlockInfo = nextNewBlock, LastCryptoBlockIndex = lastCryptoBlockIndex });
            }

            if (syncingBlocks.LastBlock.Height == lastCryptoBlockIndex)
            {
                // No new blocks.
                return Task.FromResult(default(SyncBlockOperation));
            }

            var nextHash = client.GetblockHash(syncingBlocks.LastBlock.Height + 1).Result;

            var nextBlock = client.GetBlock(nextHash).Result;

            syncingBlocks.LastBlock = nextBlock;

            return Task.FromResult(new SyncBlockOperation { BlockInfo = nextBlock, LastCryptoBlockIndex = lastCryptoBlockIndex });
        }

        private Task<SyncBlockOperation> FindBlockInternal(SyncConnection connection, SyncingBlocks syncingBlocks)
        {
            var stoper = Stopwatch.Start();

            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            var lastCryptoBlockIndex = client.GetBlockCount().Result;

            var blockToSync = this.GetNextBlockToSync(client, connection, lastCryptoBlockIndex, syncingBlocks);

            if (blockToSync != null && blockToSync.Result.BlockInfo != null)
            {
                syncingBlocks.CurrentSyncing.TryAdd(blockToSync.Result.BlockInfo.Hash, blockToSync.Result.BlockInfo);
            }
           
            stoper.Stop();

            return blockToSync;
        }

        private Task<SyncPoolTransactions> FindPoolInternal(SyncConnection connection, SyncingBlocks syncingBlocks)
        {
            var stoper = Stopwatch.Start();

            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);
            var memPool = client.GetRawMemPool().Result;

            var currentMemoryPool = new HashSet<string>(memPool);
            var currentTable = new HashSet<string>(syncingBlocks.CurrentPoolSyncing);

            var newTransactions = currentMemoryPool.Except(currentTable).ToList();
            var deleteTransaction = currentTable.Except(currentMemoryPool).ToList();

            var newTransactionsLimited = newTransactions.Count() < 1000 ? newTransactions : newTransactions.Take(1000).ToList();

            syncingBlocks.CurrentPoolSyncing.AddRange(newTransactionsLimited);
            deleteTransaction.ForEach(t => syncingBlocks.CurrentPoolSyncing.Remove(t));

            stoper.Stop();

            this.tracer.DetailedTrace("SyncPool", string.Format("Seconds = {0} - New Transactions = {1}", stoper.Elapsed.TotalSeconds, newTransactions.Count()));

            return Task.FromResult(new SyncPoolTransactions { Transactions = newTransactionsLimited });
        }

        private Task<SyncBlockTransactionsOperation> SyncBlockTransactions(BitcoinClient client, SyncConnection connection, IEnumerable<string> transactionsToSync, bool throwIfNotFound)
        {
            var transactions = new List<DecodedRawTransaction>();

            var batchListItems = transactionsToSync.Batch(this.configuration.ParallelRequestsToTransactionRpc);

            foreach (var batch in batchListItems)
            {
                var itemList = batch.ToList();

                var stoper = new System.Diagnostics.Stopwatch();
                stoper.Start();

                var waits = itemList.Select(item =>
                    {
                        try
                        {
                            var transaction = client.GetRawTransaction(item, 1);

                            return transaction;
                        }
                        catch (BitcoinClientException bce)
                        {
                            if (!throwIfNotFound && bce.IsTransactionNotFound())
                            {
                                //// the transaction was not found in the client, 
                                //// if this is a pool sync we assume the transaction was initially found in the pool and became invalid.
                                return null;
                            }

                            throw;
                        }
                    });

                var waitList = Task.WhenAll(waits).Result; //await Task.WhenAll(waits);

                var enumerateAwaits = waitList.ToList();

                transactions.AddRange(enumerateAwaits.Where(t => t != null).ToList());

                stoper.Stop();

                this.tracer.DetailedTrace("SyncBlockTransactions", string.Format("Seconds = {0} - Transactions {1} - Inputs {2} - Outputs {3} ", stoper.Elapsed.TotalSeconds, itemList.Count(), transactions.SelectMany(s => s.VIn).Count(), transactions.SelectMany(s => s.VOut).Count()));
            }

            return Task.FromResult(new SyncBlockTransactionsOperation { Transactions = transactions });
        }

        private Task<SyncBlockTransactionsOperation> SyncPoolInternal(SyncConnection connection, SyncPoolTransactions poolTransactions)
        {
            var stoper = Stopwatch.Start();

            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            var returnBlock = this.SyncBlockTransactions(client, connection, poolTransactions.Transactions, false);

            stoper.Stop();

            this.tracer.DetailedTrace("SyncPool", string.Format("Seconds = {0} - Transactions = {1}", stoper.Elapsed.TotalSeconds, returnBlock.Result.Transactions.Count()));

            return returnBlock;
        }

        private Task<SyncBlockTransactionsOperation> SyncBlockInternal(SyncConnection connection, BlockInfo block)
        {
            var stoper = Stopwatch.Start();

            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            var returnBlock = this.SyncBlockTransactions(client, connection, block.Transactions, true);

            returnBlock.Result.BlockInfo = block;

            stoper.Stop();

            this.tracer.DetailedTrace("SyncBlock", string.Format("Seconds = {0} - Transactions = {1} - BlockIndex = {2}", stoper.Elapsed.TotalSeconds, returnBlock.Result.Transactions.Count(), returnBlock.Result.BlockInfo.Height));

            return returnBlock;
        }

        #endregion
    }
}
