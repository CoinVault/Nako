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
    using System.Diagnostics;
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
        /// <summary>
        /// The storage.
        /// </summary>
        private readonly IStorage storage;

        /// <summary>
        /// The tracer.
        /// </summary>
        private readonly Tracer tracer;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly NakoConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncOperations"/> class.
        /// </summary>
        /// <param name="storage">
        /// The storage.
        /// </param>
        /// <param name="tracer">
        /// The tracer.
        /// </param>
        /// <param name="nakoConfiguration">
        /// The Configuration.
        /// </param>
        public SyncOperations(IStorage storage, Tracer tracer, NakoConfiguration nakoConfiguration)
        {
            this.configuration = nakoConfiguration;
            this.tracer = tracer;
            this.storage = storage;
        }

        #region Public Methods and Operators

        /// <summary>
        /// The sync block.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<SyncBlockOperation> FindBlock(SyncConnection connection, SyncingBlocks container)
        {
            return this.FindBlockInternal(connection, container);
        }

        /// <summary>
        /// The sync block.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<SyncPoolTransactions> FindPoolTransactions(SyncConnection connection, SyncingBlocks container)
        {
            return this.FindPoolInternal(connection, container);
        }

        /// <summary>
        /// The sync memory pool.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="poolTransactions">
        /// Transactions not confirmed yet.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<SyncBlockTransactionsOperation> SyncPool(SyncConnection connection, SyncPoolTransactions poolTransactions)
        {
            return this.SyncPoolInternal(connection, poolTransactions);
        }

        /// <summary>
        /// The sync transactions.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="block">
        /// The block.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<SyncBlockTransactionsOperation> SyncBlock(SyncConnection connection, BlockInfo block)
        {
            return this.SyncBlockInternal(connection, block);
        }

        /// <summary>
        /// The check block reorganization.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
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

        /// <summary>
        /// Get blocks to sync.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="lastCryptoBlockIndex">
        /// last block index.
        /// </param>
        /// <param name="syncingBlocks">
        /// The container.
        /// </param>
        /// <returns>
        /// Collection of blocks to sync.
        /// </returns>
        private async Task<SyncBlockOperation> GetNextBlockToSync(BitcoinClient client, SyncConnection connection, long lastCryptoBlockIndex, SyncingBlocks syncingBlocks)
        {
            if (syncingBlocks.LastBlock == null)
            {
                // because inserting blocks is sequential we'll use the indexed 'height' filed to check if the last block is incomplete.
                var incomplete = this.storage.BlockGetBlockCount(6).Where(b => !b.SyncComplete).ToList(); ////this.storage.BlockGetIncompleteBlocks().ToList();

                var incompleteToSync = incomplete.OrderBy(o => o.BlockIndex).FirstOrDefault(f => !syncingBlocks.CurrentSyncing.ContainsKey(f.BlockHash));

                if (incompleteToSync != null)
                {
                    var incompleteBlock = await client.GetBlockAsync(incompleteToSync.BlockHash);

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
                        return await Task.FromResult(default(SyncBlockOperation));
                    }

                    blockHashsToSync = await client.GetblockHashAsync(lastBlockIndex + 1);
                }
                else
                {
                    // No blocks in store start from zero configured block index.
                    blockHashsToSync = await client.GetblockHashAsync(connection.StartBlockIndex);
                }

                var nextNewBlock = await client.GetBlockAsync(blockHashsToSync);

                syncingBlocks.LastBlock = nextNewBlock;

                return new SyncBlockOperation { BlockInfo = nextNewBlock, LastCryptoBlockIndex = lastCryptoBlockIndex };
            }

            if (syncingBlocks.LastBlock.Height == lastCryptoBlockIndex)
            {
                // No new blocks.
                return await Task.FromResult(default(SyncBlockOperation));
            }

            var nextHash = await client.GetblockHashAsync(syncingBlocks.LastBlock.Height + 1);

            var nextBlock = await client.GetBlockAsync(nextHash);

            syncingBlocks.LastBlock = nextBlock;

            return new SyncBlockOperation { BlockInfo = nextBlock, LastCryptoBlockIndex = lastCryptoBlockIndex };
        }

        /// <summary>
        /// Synchronize blocks.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="syncingBlocks">
        /// The container.
        /// </param>
        /// <returns>
        /// Collection of synced blocks.
        /// </returns>
        private async Task<SyncBlockOperation> FindBlockInternal(SyncConnection connection, SyncingBlocks syncingBlocks)
        {
            var stoper = StopwatchExtension.CreateAndStart();

            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            var lastCryptoBlockIndex = await client.GetBlockCountAsync();

            var blockToSync = await this.GetNextBlockToSync(client, connection, lastCryptoBlockIndex, syncingBlocks);

            if (blockToSync != null && blockToSync.BlockInfo != null)
            {
                syncingBlocks.CurrentSyncing.TryAdd(blockToSync.BlockInfo.Hash, blockToSync.BlockInfo);
            }
           
            stoper.Stop();

            return blockToSync;
        }

        /// <summary>
        /// Sync the memory pool.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="syncingBlocks">
        /// The syncing Blocks.
        /// </param>
        /// <returns>
        /// A task.
        /// </returns>
        private async Task<SyncPoolTransactions> FindPoolInternal(SyncConnection connection, SyncingBlocks syncingBlocks)
        {
            var stoper = StopwatchExtension.CreateAndStart();

            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);
            var memPool = await client.GetRawMemPoolAsync();

            var currentMemoryPool = new HashSet<string>(memPool);
            var currentTable = new HashSet<string>(syncingBlocks.CurrentPoolSyncing);

            var newTransactions = currentMemoryPool.Except(currentTable).ToList();
            var deleteTransaction = currentTable.Except(currentMemoryPool).ToList();

            var newTransactionsLimited = newTransactions.Count() < 1000 ? newTransactions : newTransactions.Take(1000).ToList();

            syncingBlocks.CurrentPoolSyncing.AddRange(newTransactionsLimited);
            deleteTransaction.ForEach(t => syncingBlocks.CurrentPoolSyncing.Remove(t));

            stoper.Stop();

            this.tracer.DetailedTrace("SyncPool", string.Format("Seconds = {0} - New Transactions = {1}", stoper.Elapsed.TotalSeconds, newTransactions.Count()));

            return new SyncPoolTransactions { Transactions = newTransactionsLimited };
        }

        /// <summary>
        /// Sync a block transactions.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="transactionsToSync">
        /// The transactions to sync.
        /// </param>
        /// <param name="throwIfNotFound">
        /// The throw If Not Found.
        /// </param>
        /// <returns>
        /// A task operation.
        /// </returns>
        private async Task<SyncBlockTransactionsOperation> SyncBlockTransactions(BitcoinClient client, SyncConnection connection, IEnumerable<string> transactionsToSync, bool throwIfNotFound)
        {
            var transactions = new List<DecodedRawTransaction>();

            var batchListItems = transactionsToSync.Batch(this.configuration.ParallelRequestsToTransactionRpc);

            foreach (var batch in batchListItems)
            {
                var itemList = batch.ToList();

                var stoper = new Stopwatch();
                stoper.Start();

                var waits = itemList.Select(async item =>
                    {
                        try
                        {
                            var transaction = await client.GetRawTransactionAsync(item, 1);

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

                var waitList = await Task.WhenAll(waits);

                var enumerateAwaits = waitList.ToList();

                transactions.AddRange(enumerateAwaits.Where(t => t != null).ToList());

                stoper.Stop();

                this.tracer.DetailedTrace("SyncBlockTransactions", string.Format("Seconds = {0} - Transactions {1} - Inputs {2} - Outputs {3} ", stoper.Elapsed.TotalSeconds, itemList.Count(), transactions.SelectMany(s => s.VIn).Count(), transactions.SelectMany(s => s.VOut).Count()));
            }

            return new SyncBlockTransactionsOperation { Transactions = transactions };
        }

        /// <summary>
        /// Sync the memory pool.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="poolTransactions">
        /// The pool Transactions.
        /// </param>
        /// <returns>
        /// A task.
        /// </returns>
        private async Task<SyncBlockTransactionsOperation> SyncPoolInternal(SyncConnection connection, SyncPoolTransactions poolTransactions)
        {
            var stoper = StopwatchExtension.CreateAndStart();

            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            var returnBlock = await this.SyncBlockTransactions(client, connection, poolTransactions.Transactions, false);

            stoper.Stop();

            this.tracer.DetailedTrace("SyncPool", string.Format("Seconds = {0} - Transactions = {1}", stoper.Elapsed.TotalSeconds, returnBlock.Transactions.Count()));

            return returnBlock;
        }

        /// <summary>
        /// Sync transactions in a block.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="block">
        /// The block.
        /// </param>
        /// <returns>
        /// A task.
        /// </returns>
        private async Task<SyncBlockTransactionsOperation> SyncBlockInternal(SyncConnection connection, BlockInfo block)
        {
            var stoper = StopwatchExtension.CreateAndStart();

            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            var returnBlock = await this.SyncBlockTransactions(client, connection, block.Transactions, true);

            returnBlock.BlockInfo = block;

            stoper.Stop();

            this.tracer.DetailedTrace("SyncBlock", string.Format("Seconds = {0} - Transactions = {1} - BlockIndex = {2}", stoper.Elapsed.TotalSeconds, returnBlock.Transactions.Count(), returnBlock.BlockInfo.Height));

            return returnBlock;
        }

        #endregion
    }
}
