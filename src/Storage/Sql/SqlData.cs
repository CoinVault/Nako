// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlData.cs" company="SoftChains">
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
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;

    using Nako.Client.Types;
    using Nako.Config;
    using Nako.Storage.Sql.Types;
    using Nako.Storage.Types;

    #endregion

    /// <summary>
    /// NOTE:The sql functionality is not maintained.
    /// </summary>
    public class SqlData : IStorage
    {
        /// <summary>
        /// The tracer.
        /// </summary>
        private readonly Tracer tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlData"/> class.
        /// </summary>
        /// <param name="tracer">
        /// The tracer.
        /// </param>
        public SqlData(Tracer tracer)
        {
            this.tracer = tracer;
        }

        #region Public Methods and Operators

        /// <summary>
        /// The get block count.
        /// </summary>
        /// <returns>
        /// The collection.
        /// </returns>
        public IEnumerable<SyncBlockInfo> BlockGetIncompleteBlocks()
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[BlocksGetIncomplete]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                var reader = sp.ExecuteReader();

                var ret = new List<SyncBlockInfo>();

                while (reader.Read())
                {
                    ret.Add(
                        new SyncBlockInfo
                        {
                            BlockHash = reader.GetFieldValue<string>(0), 
                            BlockIndex = reader.GetFieldValue<long>(1), 
                            BlockSize = reader.GetFieldValue<long>(2), 
                            BlockTime = reader.GetFieldValue<long>(3), 
                            NextBlockHash = reader.GetFieldValueOrDefault<string>(4), 
                            PreviousBlockHash = reader.GetFieldValue<string>(5), 
                            SyncComplete = reader.GetFieldValue<bool>(6), 
                            TransactionCount = reader.GetFieldValue<int>(7)
                        });
                }

                return ret;
            }
        }

        /// <summary>
        /// The get block count.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// The collection.
        /// </returns>
        public IEnumerable<SyncBlockInfo> BlockGetBlockCount(int count)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[BlocksGetTop]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@Count", SqlDbType.Int, count);

                var reader = sp.ExecuteReader();

                var ret = new List<SyncBlockInfo>();

                while (reader.Read())
                {
                    ret.Add(
                        new SyncBlockInfo
                        {
                            BlockHash = reader.GetFieldValue<string>(0), 
                            BlockIndex = reader.GetFieldValue<long>(1), 
                            BlockSize = reader.GetFieldValue<long>(2), 
                            BlockTime = reader.GetFieldValue<long>(3), 
                            NextBlockHash = reader.GetFieldValueOrDefault<string>(4), 
                            PreviousBlockHash = reader.GetFieldValue<string>(5), 
                            SyncComplete = reader.GetFieldValue<bool>(6), 
                            TransactionCount = reader.GetFieldValue<int>(7) 
                        });
                }

                return ret;
            }
        }

        /// <summary>
        /// The get block count.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// The collection.
        /// </returns>
        public IEnumerable<SyncBlockInfo> BlockGetCompleteBlockCount(int count)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[BlocksGetTopComplete]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@Count", SqlDbType.Int, count);

                var reader = sp.ExecuteReader();

                var ret = new List<SyncBlockInfo>();

                while (reader.Read())
                {
                    ret.Add(
                        new SyncBlockInfo
                        {
                            BlockHash = reader.GetFieldValue<string>(0), 
                            BlockIndex = reader.GetFieldValue<long>(1), 
                            BlockSize = reader.GetFieldValue<long>(2), 
                            BlockTime = reader.GetFieldValue<long>(3), 
                            NextBlockHash = reader.GetFieldValueOrDefault<string>(4), 
                            PreviousBlockHash = reader.GetFieldValue<string>(5), 
                            SyncComplete = reader.GetFieldValue<bool>(6), 
                            TransactionCount = reader.GetFieldValue<int>(7)
                        });
                }

                return ret;
            }
        }

        /// <summary>
        /// The block get by hash.
        /// </summary>
        /// <param name="blockHash">
        /// The block hash.
        /// </param>
        public void InvalidateBlock(string blockHash)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[InvalidateBlock]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@BlockHash", SqlDbType.NVarChar, blockHash);

                sp.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// The block get by hash.
        /// </summary>
        /// <param name="blockHash">
        /// The block hash.
        /// </param>
        /// <returns>
        /// The <see cref="SyncBlockInfo"/>.
        /// </returns>
        public SyncBlockInfo BlockGetByHash(string blockHash)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[BlocksGetByHash]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@BlockHash", SqlDbType.NVarChar, blockHash);

                var reader = sp.ExecuteReader();

                var ret = new List<SyncBlockInfo>();

                while (reader.Read())
                {
                    ret.Add(
                        new SyncBlockInfo
                        {
                            BlockHash = reader.GetFieldValue<string>(0), 
                            BlockIndex = reader.GetFieldValue<long>(1), 
                            BlockSize = reader.GetFieldValue<long>(2), 
                            BlockTime = reader.GetFieldValue<long>(3), 
                            NextBlockHash = reader.GetFieldValueOrDefault<string>(4), 
                            PreviousBlockHash = reader.GetFieldValue<string>(5), 
                            SyncComplete = reader.GetFieldValue<bool>(6), 
                            TransactionCount = reader.GetFieldValue<int>(7) 
                        });
                }

                return ret.SingleOrDefault();
            }
        }

        /// <summary>
        /// The insert or update async.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        public void BlockInsertOrUpdate(SyncBlockInfo info)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[BlocksInsertOrUpdate]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@BlockHash", SqlDbType.NVarChar, info.BlockHash);
                sp.AddIn("@BlockIndex", SqlDbType.BigInt, info.BlockIndex);
                sp.AddIn("@BlockSize", SqlDbType.BigInt, info.BlockSize);
                sp.AddIn("@BlockTime", SqlDbType.BigInt, info.BlockTime);
                sp.AddIn("@NextBlockHash", SqlDbType.NVarChar, info.NextBlockHash);
                sp.AddIn("@PreviousBlockHash", SqlDbType.NVarChar, info.PreviousBlockHash);
                sp.AddIn("@TransactionCount", SqlDbType.Int, info.TransactionCount);
                sp.AddIn("@SyncComplete", SqlDbType.BigInt, info.SyncComplete);

                sp.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// The block transaction get.
        /// </summary>
        /// <param name="transactionId">
        /// The transaction id.
        /// </param>
        /// <returns>
        /// The <see cref="SyncTransactionInfo"/>.
        /// </returns>
        public SyncTransactionInfo BlockTransactionGet(string transactionId)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[BlockTransactionsSelect]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@TransactionHash", SqlDbType.NVarChar, transactionId);

                var reader = sp.ExecuteReader();

                var ret = new List<SyncTransactionInfo>();

                while (reader.Read())
                {
                    ret.Add(
                        new SyncTransactionInfo
                        {
                            BlockHash = reader.GetFieldValueOrDefault<string>(0), 
                            BlockIndex = reader.GetFieldValueOrDefault<long>(1), 
                            TransactionHash = reader.GetFieldValue<string>(2), 
                            Timestamp = reader.GetFieldValue<long>(3), 
                            Confirmations = reader.GetFieldValue<long>(4)
                        });
                }

                return ret.SingleOrDefault();
            }
        }

        /// <summary>
        /// The block transaction get by block.
        /// </summary>
        /// <param name="blockHash">
        /// The block hash.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<SyncTransactionInfo> BlockTransactionGetByBlock(string blockHash)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[BlockTransactionsGetByBlock]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@BlockHash", SqlDbType.NVarChar, blockHash);

                var reader = sp.ExecuteReader();

                var ret = new List<SyncTransactionInfo>();

                while (reader.Read())
                {
                    ret.Add(
                        new SyncTransactionInfo
                        {
                            BlockHash = reader.GetFieldValue<string>(0), 
                            BlockIndex = reader.GetFieldValueOrDefault<long>(1), 
                            TransactionHash = reader.GetFieldValue<string>(2), 
                            Timestamp = reader.GetFieldValue<long>(3) 
                        });
                }

                return ret;
            }
        }

        /// <summary>
        /// The block transaction get by block.
        /// </summary>
        /// <param name="blockIndex">
        /// The block index.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<SyncTransactionInfo> BlockTransactionGetByBlockIndex(long blockIndex)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[BlockTransactionsGetByBlockIndex]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@BlockIndex", SqlDbType.BigInt, blockIndex);

                var reader = sp.ExecuteReader();

                var ret = new List<SyncTransactionInfo>();

                while (reader.Read())
                {
                    ret.Add(
                        new SyncTransactionInfo
                        {
                            BlockHash = reader.GetFieldValue<string>(0), 
                            BlockIndex = reader.GetFieldValueOrDefault<long>(1), 
                            TransactionHash = reader.GetFieldValue<string>(2), 
                            Timestamp = reader.GetFieldValue<long>(3)
                        });
                }

                return ret;
            }
        }

        /// <summary>
        /// The insert batch.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        public void TransactionsInsertBatch(IEnumerable<SyncTransactionInfo> collection)
        {
            var stoper = new Stopwatch();
            stoper.Start();

            using (var conn = new SqlConnection(Common.ConnectionString))
            using (var sp = new SqlCommand("delete from [Intermediary].[Transactions]", conn))
            {
                conn.Open();
                sp.ExecuteNonQuery();
            }

            var table = new DataTable();
            table.Columns.Add("TransactionHash", typeof(string));
            table.Columns.Add("Timestamp", typeof(long));

            foreach (var row in collection)
            {
                table.Rows.Add(////row.BlockHash, 
                    ////row.BlockIndex, 
                    row.TransactionHash, row.Timestamp);
            }
          
            using (var copier = new SqlBulkCopy(Common.ConnectionString))
            {
                copier.DestinationTableName = "[Intermediary].[Transactions]";
                copier.BulkCopyTimeout = 600;
                copier.WriteToServer(table);
            }

            using (var sp = new StoredProcedure(Common.ConnectionString, "[Intermediary].[TransactionInserts]"))
            {
                sp.ExecuteNonQuery();
            }

            this.tracer.DetailedTrace("BlockTransactionInsertBatch", string.Format("Execution {0} seconds", stoper.Elapsed.TotalSeconds));
        }

        /// <summary>
        /// The transactions get.
        /// </summary>
        /// <param name="transactionId">
        /// The transaction id.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="indexType">
        /// The index type.
        /// </param>
        /// <returns>
        /// The <see cref="SyncTransactionItemOutput"/>.
        /// </returns>
        public SyncTransactionItemOutput TransactionsGet(string transactionId, int index, SyncTransactionIndexType indexType)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[dbo].[TransactionsSelectByIndex]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@TransactionId", SqlDbType.NVarChar, transactionId);
                sp.AddIn("@Index", SqlDbType.Int, index);
                sp.AddIn("@IndexType", SqlDbType.Int, indexType);

                var reader = sp.ExecuteReader();

                var ret = new List<SyncTransactionItemOutput>();

                while (reader.Read())
                {
                    ret.Add(
                    new SyncTransactionItemOutput());
                }

                return ret.SingleOrDefault();
            }
        }

        /// <summary>
        /// The transactions get.
        /// </summary>
        /// <param name="transactionId">
        /// The transaction id.
        /// </param>
        /// <returns>
        /// The <see cref="SyncTransactionItems"/>.
        /// </returns>
        public SyncTransactionItems TransactionItemsGet(string transactionId)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[TransactionItemsSelect]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@TransactionHash", SqlDbType.NVarChar, transactionId);

                var reader = sp.ExecuteReader();

                var ret = new SyncTransactionItems { Inputs = new List<SyncTransactionItemInput>(), Outputs = new List<SyncTransactionItemOutput>() };

                while (reader.Read())
                {
                    ret.Outputs.Add(
                    new SyncTransactionItemOutput
                    {
                        Index = reader.GetFieldValue<int>(1), 
                        Address = reader.GetFieldValue<string>(2), 
                        OutputType = reader.GetFieldValue<string>(3), 
                        Value = reader.GetFieldValue<decimal>(4)
                    });
                }

                reader.NextResult();

                while (reader.Read())
                {
                    ret.Inputs.Add(
                    new SyncTransactionItemInput
                    {
                        PreviousTransactionHash = reader.GetFieldValue<string>(1), 
                        PreviousIndex = reader.GetFieldValue<int>(2)
                    });
                }

                reader.NextResult();

                while (reader.Read())
                {
                    ret.Inputs.Add(
                    new SyncTransactionItemInput
                    {
                        InputCoinBase = reader.GetFieldValue<string>(1)
                    });
                }

                return ret;
            }
        }

        /// <summary>
        /// The transactions get.
        /// </summary>
        /// <param name="address">
        /// The transaction id.
        /// </param>
        /// <param name="confirmations">
        /// The count the transaction is considered confirmed.
        /// </param>
        /// <returns>
        /// The collection.
        /// </returns>
        public SyncTransactionAddressBalance AddressGetBalance(string address, long confirmations)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[TransactionGetByAddress]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@Address", SqlDbType.NVarChar, address);
                sp.AddIn("@Confirmations", SqlDbType.BigInt, confirmations);

                var reader = sp.ExecuteReader();

                var ret = new List<SyncTransactionAddressItem>();

                while (reader.Read())
                {
                    ret.Add(
                    new SyncTransactionAddressItem
                    {
                        TransactionHash = reader.GetFieldValue<string>(0), 
                        Index = reader.GetFieldValue<int>(1), 
                        Address = reader.GetFieldValue<string>(2), 
                        Type = reader.GetFieldValue<string>(3), 
                        Value = reader.GetFieldValue<decimal>(4), 
                        SpendingTransactionHash = reader.GetFieldValueOrDefault<string>(5), 
                        CoinBase = reader.GetFieldValueOrDefault<string>(6), 
                        BlockIndex = reader.GetFieldValueOrDefault<long?>(7), 
                        Confirmations = reader.GetFieldValueOrDefault<long?>(8), 
                        Time = reader.GetFieldValueOrDefault<long>(9)
                    });
                }

                return new SyncTransactionAddressBalance { Items = ret };
            }
        }

        /// <summary>
        /// The transactions get.
        /// </summary>
        /// <param name="address">
        /// The transaction id.
        /// </param>
        /// <param name="confirmations">
        /// The count the transaction is considered confirmed.
        /// </param>
        /// <returns>
        /// The collection.
        /// </returns>
        public SyncTransactionAddressBalance AddressGetBalanceWithUnconfirmedTransactions(string address, long confirmations)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[AddressGetUnconfirmed]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@Address", SqlDbType.NVarChar, address);
                sp.AddIn("@Confirmations", SqlDbType.BigInt, confirmations);

                var reader = sp.ExecuteReader();

                var ret = new List<SyncTransactionAddressItem>();

                while (reader.Read())
                {
                    ret.Add(
                    new SyncTransactionAddressItem
                    {
                        TransactionHash = reader.GetFieldValue<string>(0), 
                        Index = reader.GetFieldValue<int>(1), 
                        Address = reader.GetFieldValue<string>(2), 
                        Type = reader.GetFieldValue<string>(3), 
                        Value = reader.GetFieldValue<decimal>(4), 
                        SpendingTransactionHash = reader.GetFieldValueOrDefault<string>(5), 
                        CoinBase = reader.GetFieldValueOrDefault<string>(6), 
                        BlockIndex = reader.GetFieldValueOrDefault<long?>(7), 
                        Confirmations = reader.GetFieldValueOrDefault<long?>(8), 
                        Time = reader.GetFieldValueOrDefault<long>(9)
                    });
                }

                return new SyncTransactionAddressBalance { Items = ret };
            }
        }

        /// <summary>
        /// The transactions get.
        /// </summary>
        /// <param name="address">
        /// The transaction id.
        /// </param>
        /// <param name="confirmations">
        /// The count the transaction is considered confirmed.
        /// </param>
        /// <returns>
        /// The collection.
        /// </returns>
        public SyncTransactionAddressBalance AddressGetBalanceWithUnspentTransactions(string address, long confirmations)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[TransactionUnspentGetByAddress]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@Address", SqlDbType.NVarChar, address);
                sp.AddIn("@Confirmations", SqlDbType.BigInt, confirmations);

                var reader = sp.ExecuteReader();

                var ret = new List<SyncTransactionAddressItem>();

                while (reader.Read())
                {
                    ret.Add(
                    new SyncTransactionAddressItem
                    {
                        TransactionHash = reader.GetFieldValue<string>(0), 
                        Index = reader.GetFieldValue<int>(1), 
                        Address = reader.GetFieldValue<string>(2), 
                        Type = reader.GetFieldValue<string>(3), 
                        Value = reader.GetFieldValue<decimal>(4), 
                        SpendingTransactionHash = reader.GetFieldValueOrDefault<string>(5), 
                        CoinBase = reader.GetFieldValueOrDefault<string>(6), 
                        BlockIndex = reader.GetFieldValueOrDefault<long?>(7), 
                        Confirmations = reader.GetFieldValueOrDefault<long?>(8), 
                        Time = reader.GetFieldValueOrDefault<long>(9)
                    });
                }

                return new SyncTransactionAddressBalance { Items = ret };
            }
        }

        /// <summary>
        /// The transactions get.
        /// </summary>
        /// <param name="address">
        /// The transaction id.
        /// </param>
        /// <param name="confirmations">
        /// The count the transaction is considered confirmed.
        /// </param>
        /// <returns>
        /// The collection.
        /// </returns>
        public SyncTransactionAddressBalance AddressGetBalanceUtxo(string address, long confirmations)
        {
            var connectionString = Common.ConnectionString;
            var storedProc = "[Blocks].[TransactionGetBalanceByAddress]";

            using (var sp = new StoredProcedure(connectionString, storedProc))
            {
                sp.AddIn("@Address", SqlDbType.NVarChar, address);
                sp.AddIn("@Confirmations", SqlDbType.BigInt, confirmations);

                var reader = sp.ExecuteReader();

                var ret = new List<SyncTransactionAddressBalance>();

                while (reader.Read())
                {
                    ret.Add(
                    new SyncTransactionAddressBalance
                    {
                        Received = reader.GetFieldValueOrDefault<decimal>(0), 
                        Sent = reader.GetFieldValueOrDefault<decimal>(1), 
                        Unconfirmed = reader.GetFieldValueOrDefault<decimal>(2)
                    });
                }

                return ret.SingleOrDefault();
            }
        }

        /// <summary>
        /// The transactions insert batch.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        public void TransactionInsertItemsBatch(IEnumerable<SyncTransactionIntermediary> collection)
        {
            var stoper = new Stopwatch();
            stoper.Start();

            using (var conn = new SqlConnection(Common.ConnectionString))
            using (var sp = new SqlCommand("delete from [Intermediary].[TransactionItems]", conn))
            {
                conn.Open();
                sp.ExecuteNonQuery();
            }

            var table = new DataTable();
            table.Columns.Add("TransactionHash", typeof(string));
            table.Columns.Add("Index", typeof(int));
            table.Columns.Add("IndexType", typeof(int));
            table.Columns.Add("Timestamp", typeof(DateTime));
            table.Columns.Add("InputCoinBase", typeof(string));
            table.Columns.Add("InputIndex", typeof(int));
            table.Columns.Add("InputTransactionHash", typeof(string));
            table.Columns.Add("InputAddress", typeof(string));
            table.Columns.Add("InputValue", typeof(decimal));
            table.Columns.Add("OutputAddress", typeof(string));
            table.Columns.Add("OutputType", typeof(string));
            table.Columns.Add("OutputValue", typeof(decimal));

            foreach (var row in collection)
            {
                table.Rows.Add(
                    row.TransactionId, 
                    row.Index, 
                    row.IndexType, 
                    row.Timestamp, 
                    row.InputCoinBase, 
                    row.InputIndex, 
                    row.InputTransactionHash, 
                    row.InputAddress, 
                    row.InputValue, 
                    row.OutputAddress, 
                    row.OutputType, 
                    row.OutputValue);
            }

            using (var copier = new SqlBulkCopy(Common.ConnectionString))
            {
                copier.DestinationTableName = "[Intermediary].[TransactionItems]";
                copier.BulkCopyTimeout = 600;
                copier.WriteToServer(table);
            }

            using (var sp = new StoredProcedure(Common.ConnectionString, "[Intermediary].[TransactionItemInserts]"))
            {
                sp.ExecuteNonQuery();
            }

            this.tracer.DetailedTrace("TransactionsInsertBatch", string.Format("Execution {0} seconds", stoper.Elapsed.TotalSeconds));
        }

        /// <summary>
        /// The transactions merge batch.
        /// </summary>
        public void TransactionItemsMergeBatch()
        {
            var stoper = new Stopwatch();
            stoper.Start();

            using (var sp = new StoredProcedure(Common.ConnectionString, "[Intermediary].[TransactionMergeInserts]"))
            {
                sp.ExecuteNonQuery();
            }

            this.tracer.DetailedTrace("TransactionsMergeBatch", string.Format("Execution {0} seconds", stoper.Elapsed.TotalSeconds));
        }

        /// <summary>
        /// The transaction items associate block batch.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int ClearUnconfirmedTransactions()
        {
            var stoper = new Stopwatch();
            stoper.Start();
            int ret = 0;
            using (var sp = new StoredProcedure(Common.ConnectionString, "[Blocks].[ClearUnconfirmedTransactions]"))
            {
                ret = sp.ExecuteNonQuery();
            }

            this.tracer.DetailedTrace("ClearUnconfirmedTransactions", string.Format("Execution {0} seconds", stoper.Elapsed.TotalSeconds));

            return ret;
        }

        /// <summary>
        /// The transaction items associate block batch.
        /// </summary>
        /// <param name="blockIndex">
        /// The block index.
        /// </param>
        public void TransactionsAssociateBlockBatch(long blockIndex)
        {
            var stoper = new Stopwatch();
            stoper.Start();

            using (var sp = new StoredProcedure(Common.ConnectionString, "[Intermediary].[AssociateBlock]"))
            {
                sp.AddIn("@BlockIndex", SqlDbType.BigInt, blockIndex);

                sp.ExecuteNonQuery();
            }

            this.tracer.DetailedTrace("TransactionsAssociateBlockBatch", string.Format("Execution {0} seconds", stoper.Elapsed.TotalSeconds));
        }

        #endregion

        /// <summary>
        /// The block get by index.
        /// </summary>
        /// <param name="blockIndex">
        /// The block index.
        /// </param>
        /// <returns>
        /// The <see cref="SyncBlockInfo"/>.
        /// </returns>
        public SyncBlockInfo BlockGetByIndex(long blockIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The delete block.
        /// </summary>
        /// <param name="blockHash">
        /// The block hash.
        /// </param>
        public void DeleteBlock(string blockHash)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The get memory transactions.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<DecodedRawTransaction> GetMemoryTransactions()
        {
            throw new NotImplementedException();
        }
    }
}
