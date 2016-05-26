// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageOperations.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage.Sql
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Nako.Client.Types;
    using Nako.Config;
    using Nako.Extensions;
    using Nako.Operations;
    using Nako.Operations.Types;
    using Nako.Storage.Sql.Types;
    using Nako.Storage.Types;
    using Nako.Sync;

    #endregion

    /// <summary>
    /// The CoinOperations interface.
    /// </summary>
    public class StorageOperations : IStorageOperations
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
        /// The data.
        /// </summary>
        private readonly SqlData data;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageOperations"/> class.
        /// </summary>
        /// <param name="storage">
        /// The storage.
        /// </param>
        /// <param name="sqlData">
        /// The Data.
        /// </param>
        /// <param name="tracer">
        /// The tracer.
        /// </param>
        /// <param name="nakoConfiguration">
        /// The Configuration.
        /// </param>
        public StorageOperations(IStorage storage, SqlData sqlData, Tracer tracer, NakoConfiguration nakoConfiguration)
        {
            this.data = sqlData;
            this.configuration = nakoConfiguration;
            this.tracer = tracer;
            this.storage = storage;
        }

        #region Public Methods and Operators

        /// <summary>
        /// The validate block.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
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
                            throw new ApplicationException("This should never happen.");
                        }
                    }
                    else
                    {
                        if (item.BlockInfo.PreviousBlockHash != lastBlock.BlockHash)
                        {
                            this.InvalidBlockFound(lastBlock, item);
                        }

                        this.CreateBlock(item.BlockInfo);

                        ////if (string.IsNullOrEmpty(lastBlock.NextBlockHash))
                        ////{
                        ////    lastBlock.NextBlockHash = item.BlockInfo.Hash;
                        ////    this.SyncOperations.UpdateBlockHash(lastBlock);
                        ////}
                    }
                }
                else
                {
                    this.CreateBlock(item.BlockInfo);
                }
            }
        }

        /// <summary>
        /// The insert transactions.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public InsertStats InsertTransactions(SyncBlockTransactionsOperation item)
        {
            //// Consider looping over the transaction list and batching it.
            //// This can be achieved by batching - loop over batches of 1000 
            //// (example - item.Transactions.Batch(1000) then foreach that and call the code bellow before doing the BlockInfo work)
            //// This will reduce the work on the db
            List<SyncTransactionInfo> transactions = new List<SyncTransactionInfo>();
            List<SyncTransactionIntermediary> transactionsItems = new List<SyncTransactionIntermediary>();
            var queue = new Queue<DecodedRawTransaction>(item.Transactions);
            do
            {
                var items = this.GetTransactionBatch(queue).ToList();

                var innerTransactions = this.CreateTransactions(item.BlockInfo, items).ToList();
                this.data.TransactionsInsertBatch(innerTransactions);
                transactions.AddRange(innerTransactions);

                var innerTransactionsItems = this.CreateTransactionItems(items).ToList();
                this.data.TransactionInsertItemsBatch(innerTransactionsItems);
                transactionsItems.AddRange(innerTransactionsItems);

                if (item.BlockInfo != null)
                {
                    this.data.TransactionsAssociateBlockBatch(item.BlockInfo.Height);
                }
            }
            while (queue.Any());

            if (item.BlockInfo != null)
            {
                this.CompleteBlock(item.BlockInfo);
            }

            return new InsertStats { Transactions = transactionsItems.Count() + transactions.Count() };
        }
        
        #endregion

        /// <summary>
        /// The complete block.
        /// </summary>
        /// <param name="block">
        /// The block.
        /// </param>
        private void CompleteBlock(BlockInfo block)
        {
            var blockData = this.storage.BlockGetByHash(block.Hash);

            blockData.SyncComplete = true;

            this.data.BlockInsertOrUpdate(blockData);
        }

        /// <summary>
        /// The complete block.
        /// </summary>
        /// <param name="block">
        /// The block.
        /// </param>
        private void CreateBlock(BlockInfo block)
        {
            var blockInfo = new SyncBlockInfo
            {
                BlockIndex = block.Height, 
                BlockHash = block.Hash, 
                BlockSize = block.Size, 
                BlockTime = block.Time, 
                NextBlockHash = block.NextBlockHash, 
                PreviousBlockHash = block.PreviousBlockHash, 
                TransactionCount = block.Transactions.Count(), 
                SyncComplete = false
            };

            this.data.BlockInsertOrUpdate(blockInfo);
        }

        /// <summary>
        /// Gets a batch of transactions.
        /// </summary>
        /// <param name="queue">
        /// The transaction queue.
        /// </param>
        /// <returns>
        /// A batch of transactions.
        /// </returns>
        private IEnumerable<DecodedRawTransaction> GetTransactionBatch(Queue<DecodedRawTransaction> queue)
        {
            var maxItems = 5000;
            var total = 0;
            var items = new List<DecodedRawTransaction>();

            do
            {
                var aggregate = Nako.Extensions.Extensions.TakeAndRemove(queue, 100).ToList();

                items.AddRange(aggregate);

                total = items.SelectMany(s => s.VIn).Cast<object>().Concat(items.SelectMany(s => s.VOut).Cast<object>()).Count();
            }
            while (total < maxItems && queue.Any());

            return items;
        }

        /// <summary>
        /// The invalid block found.
        /// </summary>
        /// <param name="lastBlock">
        /// The last block.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        private void InvalidBlockFound(SyncBlockInfo lastBlock, SyncBlockTransactionsOperation item)
        {
            // Re-org happened.
            this.data.InvalidateBlock(lastBlock.BlockHash);
           
            throw new SyncRestartException();
        }

        /// <summary>
        /// The create transaction inserts.
        /// </summary>
        /// <param name="block">
        /// The block.
        /// </param>
        /// <param name="transactions">
        /// The transactions.
        /// </param>
        /// <returns>
        /// The collection.
        /// </returns>
        private IEnumerable<SyncTransactionInfo> CreateTransactions(BlockInfo block, IEnumerable<DecodedRawTransaction> transactions)
        {
            var trxInfps = transactions.Select(trx => new SyncTransactionInfo
            {
                TransactionHash = trx.TxId, 
                Timestamp = block == null ? UnixUtils.DateToUnixTimestamp(DateTime.UtcNow) : block.Time
            });

            return trxInfps;
        }

        /// <summary>
        /// The create transaction inserts.
        /// </summary>
        /// <param name="transactions">
        /// The block.
        /// </param>
        /// <returns>
        /// The collection.
        /// </returns>
        private IEnumerable<SyncTransactionIntermediary> CreateTransactionItems(IEnumerable<DecodedRawTransaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                var rawTransaction = transaction;

                var transactionInputs = from input in transaction.VIn.Select((vin, index) => new { Item = vin, Index = index })
                                        where input.Item.CoinBase != null || input.Item.TxId != null
                                        select new SyncTransactionIntermediary
                                        {
                                            IndexType = SyncTransactionIndexType.Input, 
                                            TransactionId = rawTransaction.TxId, 
                                            Index = input.Index, 
                                            InputCoinBase = input.Item.CoinBase, 
                                            InputTransactionHash = input.Item.TxId, 
                                            InputIndex = input.Item.VOut
                                        };

                foreach (var input in transactionInputs)
                {
                    yield return input;
                }

                var transactionOutputs = from output in transaction.VOut
                                         where output.Value > 0
                                                 && output.ScriptPubKey != null
                                                 && output.ScriptPubKey.Addresses != null
                                                 && output.ScriptPubKey.Addresses.Any()
                                         select new SyncTransactionIntermediary
                                         {
                                             IndexType = SyncTransactionIndexType.Output, 
                                             TransactionId = rawTransaction.TxId, 
                                             OutputValue = output.Value, 
                                             OutputType = output.ScriptPubKey.Type, 
                                             Index = output.N, 
                                             OutputAddress = output.ScriptPubKey.Addresses.FirstOrDefault()
                                         };

                foreach (var output in transactionOutputs)
                {
                    yield return output;
                }
            }
        }
    }
}
