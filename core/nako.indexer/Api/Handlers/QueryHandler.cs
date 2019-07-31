// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryHandler.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Options;

namespace Nako.Api.Handlers
{
    using System.Collections.Generic;
    #region Using Directives

    using System.Linq;

    using Nako.Api.Handlers.Types;
    using Nako.Config;
    using Nako.Extensions;
    using Nako.Storage;

    #endregion

    /// <summary>
    /// A handler that make request on the blockchain.
    /// </summary>
    public class QueryHandler 
    {
        private readonly NakoConfiguration configuration;

        private readonly IStorage storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryHandler"/> class.
        /// </summary>
        public QueryHandler(IOptions<NakoConfiguration> configuration, IStorage storage)
        {
            this.storage = storage;
            this.configuration = configuration.Value;
        }

        public QueryAddress GetAddressTransactions(string address, long confirmations)
        {
            var stats = this.storage.AddressGetBalance(address, confirmations);

            if (stats == null)
            {
                return new QueryAddress();
            }

            // filter
            var transactions = stats.Items.ToList();
            var confirmed = transactions.Where(t => t.Confirmations >= confirmations).ToList();
            var unconfirmed = transactions.Where(t => t.Confirmations < confirmations).ToList();

            return new QueryAddress
            {
                CoinTag = this.configuration.CoinTag,
                Address = address,
                Balance = stats.Available,
                TotalReceived = stats.Received,
                TotalSent = stats.Sent,
                UnconfirmedBalance = stats.Unconfirmed,
                Transactions = confirmed.Select(t => new QueryAddressItem
                {
                    PubScriptHex = t.ScriptHex,
                    CoinBase = t.CoinBase,
                    Index = t.Index,
                    SpendingTransactionHash = t.SpendingTransactionHash,
                    TransactionHash = t.TransactionHash,
                    Type = t.Type, Value = t.Value,
                    BlockIndex = t.BlockIndex,
                    Confirmations = t.Confirmations,
                    Time = t.Time
                }),
                UnconfirmedTransactions = unconfirmed.Select(t => new QueryAddressItem
                {
                    PubScriptHex = t.ScriptHex,
                    CoinBase = t.CoinBase, Index = t.Index,
                    SpendingTransactionHash = t.SpendingTransactionHash,
                    TransactionHash = t.TransactionHash,
                    Type = t.Type, Value = t.Value,
                    BlockIndex = t.BlockIndex,
                    Confirmations = t.Confirmations,
                    Time = t.Time
                })
            };
        }

        public QueryAddress GetAddress(string address, long confirmations)
        {
            var stats = this.storage.AddressGetBalance(address, confirmations);

            if (stats == null)
            {
                return new QueryAddress();
            }

            return new QueryAddress
            {
                CoinTag = this.configuration.CoinTag, 
                Address = address, 
                Balance = stats.Available, 
                TotalReceived = stats.Received, 
                TotalSent = stats.Sent, 
                UnconfirmedBalance = stats.Unconfirmed, 
                Transactions = Enumerable.Empty<QueryAddressItem>(), 
                UnconfirmedTransactions = Enumerable.Empty<QueryAddressItem>()
            };
        }

        public QueryAddress GetAddressUtxo(string address, long confirmations)
        {
            var stats = this.storage.AddressGetBalanceUtxo(address, confirmations);

            if (stats == null)
            {
                return new QueryAddress();
            }

            return new QueryAddress
            {
                CoinTag = this.configuration.CoinTag, 
                Address = address, 
                Balance = stats.Available, 
                TotalReceived = stats.Received, 
                TotalSent = stats.Sent, 
                UnconfirmedBalance = stats.Unconfirmed, 
                Transactions = Enumerable.Empty<QueryAddressItem>(), 
                UnconfirmedTransactions = Enumerable.Empty<QueryAddressItem>()
            };
        }

        public QueryAddress GetAddressUtxoTransactions(string address, long confirmations)
        {
            var stats = this.storage.AddressGetBalanceUtxo(address, confirmations);

            if (stats == null)
            {
                return new QueryAddress();
            }

            // filter
            var transactions = stats.Items.ToList();
            var confirmed = transactions.Where(t => t.Confirmations >= confirmations).ToList();
            var unconfirmed = transactions.Where(t => t.Confirmations < confirmations).ToList();

            return new QueryAddress
            {
                CoinTag = this.configuration.CoinTag, 
                Address = address, 
                Balance = stats.Available, 
                TotalReceived = stats.Received, 
                TotalSent = stats.Sent, 
                UnconfirmedBalance = stats.Unconfirmed, 
                Transactions = confirmed.Select(t => new QueryAddressItem { PubScriptHex = t.ScriptHex, CoinBase = t.CoinBase, Index = t.Index, SpendingTransactionHash = t.SpendingTransactionHash, TransactionHash = t.TransactionHash, Type = t.Type, Value = t.Value, BlockIndex = t.BlockIndex, Confirmations = t.Confirmations, Time = t.Time }), 
                UnconfirmedTransactions = unconfirmed.Select(t => new QueryAddressItem { PubScriptHex = t.ScriptHex, CoinBase = t.CoinBase, Index = t.Index, SpendingTransactionHash = t.SpendingTransactionHash, TransactionHash = t.TransactionHash, Type = t.Type, Value = t.Value, BlockIndex = t.BlockIndex, Confirmations = t.Confirmations, Time = t.Time })
            };
        }

        public QueryAddress GetAddressUtxoUnconfirmedTransactions(string address, long confirmations)
        {
            var stats = this.storage.AddressGetBalanceUtxo(address, confirmations);

            if (stats == null)
            {
                return new QueryAddress();
            }

            // filter
            var transactions = stats.Items
                .Where(s => s.Confirmations < confirmations)
                .ToList();

            return new QueryAddress
            {
                CoinTag = this.configuration.CoinTag, 
                Address = address, 
                Balance = stats.Available, 
                TotalReceived = stats.Received, 
                TotalSent = stats.Sent, 
                UnconfirmedBalance = stats.Unconfirmed, 
                Transactions = Enumerable.Empty<QueryAddressItem>(), 
                UnconfirmedTransactions = transactions.Select(t => new QueryAddressItem { PubScriptHex = t.ScriptHex, CoinBase = t.CoinBase, Index = t.Index, SpendingTransactionHash = t.SpendingTransactionHash, TransactionHash = t.TransactionHash, Type = t.Type, Value = t.Value, BlockIndex = t.BlockIndex, Confirmations = t.Confirmations, Time = t.Time })
            };
        }

        public QueryAddress GetAddressUtxoConfirmedTransactions(string address, long confirmations)
        {
            var stats = this.storage.AddressGetBalanceUtxo(address, confirmations);

            if (stats == null)
            {
                return new QueryAddress();
            }

            // filter
            var transactions = stats.Items
                .Where(s => s.Confirmations >= confirmations)
                .ToList();

            return new QueryAddress
            {
                CoinTag = this.configuration.CoinTag, 
                Address = address, 
                Balance = stats.Available, 
                TotalReceived = stats.Received, 
                TotalSent = stats.Sent, 
                UnconfirmedBalance = stats.Unconfirmed, 
                Transactions = transactions.Select(t => new QueryAddressItem { PubScriptHex = t.ScriptHex, CoinBase = t.CoinBase, Index = t.Index, SpendingTransactionHash = t.SpendingTransactionHash, TransactionHash = t.TransactionHash, Type = t.Type, Value = t.Value, BlockIndex = t.BlockIndex, Confirmations = t.Confirmations, Time = t.Time }), 
                UnconfirmedTransactions = Enumerable.Empty<QueryAddressItem>()
            };
        }

        public QueryBlock GetBlock(string blockHash, bool getTransactions = true)
        {
            var block = this.storage.BlockGetByHash(blockHash);

            if (block == null)
            {
                return new QueryBlock();
            }

            var queryBlock = new QueryBlock
            {
                CoinTag = this.configuration.CoinTag,
                BlockHash = block.BlockHash,
                BlockIndex = block.BlockIndex,
                BlockSize = block.BlockSize,
                BlockTime = block.BlockTime,
                NextBlockHash = block.NextBlockHash,
                PreviousBlockHash = block.PreviousBlockHash,
                Synced = block.SyncComplete,
                TransactionCount = block.TransactionCount,
                Bits = block.Bits,
                Confirmations = block.Confirmations,
                Merkleroot = block.Merkleroot,
                Nonce = block.Nonce,
                PosBlockSignature = block.PosBlockSignature,
                PosBlockTrust = block.PosBlockTrust,
                PosChainTrust = block.PosChainTrust,
                PosFlags = block.PosFlags,
                PosHashProof = block.PosHashProof,
                PosModifierv2 = block.PosModifierv2,
                Version = block.Version,
                Transactions = Enumerable.Empty<string>()
            };

            if (getTransactions)
            {
                var transactions = this.storage.BlockTransactionGetByBlockIndex(block.BlockIndex);
                queryBlock.Transactions = transactions.Select(s => s.TransactionHash);
            }

            return queryBlock;
        }

        public QueryBlocks GetBlocks(long blockIndex, int count)
        {
            var blocks = new List<QueryBlock>();

            if (blockIndex == -1)
            {
                QueryBlock lastBlock = this.GetLastBlock(false);
                blocks.Add(lastBlock);
                blockIndex = lastBlock.BlockIndex - 1;
                count--;
            }

            for (long i = 0; i < count; i++)
            {
                blocks.Add(this.GetBlock((int)blockIndex - i, false));
            }

            return new QueryBlocks
            {
                Blocks = blocks
            };
        }

        public QueryBlock GetBlock(long blockIndex, bool getTransactions = true)
        {
            var block = this.storage.BlockGetByIndex(blockIndex);

            if (block == null)
            {
                return new QueryBlock();
            }

            var queryBlock = new QueryBlock
            {
                CoinTag = this.configuration.CoinTag,
                BlockHash = block.BlockHash,
                BlockIndex = block.BlockIndex,
                BlockSize = block.BlockSize,
                BlockTime = block.BlockTime,
                NextBlockHash = block.NextBlockHash,
                PreviousBlockHash = block.PreviousBlockHash,
                Synced = block.SyncComplete,
                TransactionCount = block.TransactionCount,
                Bits = block.Bits,
                Confirmations = block.Confirmations,
                Merkleroot = block.Merkleroot,
                Nonce = block.Nonce,
                PosBlockSignature = block.PosBlockSignature,
                PosBlockTrust = block.PosBlockTrust,
                PosChainTrust = block.PosChainTrust,
                PosFlags = block.PosFlags,
                PosHashProof = block.PosHashProof,
                PosModifierv2 = block.PosModifierv2,
                Version = block.Version,
                Transactions = Enumerable.Empty<string>()
            };

            if (getTransactions)
            {
                var transactions = this.storage.BlockTransactionGetByBlockIndex(block.BlockIndex);
                queryBlock.Transactions = transactions.Select(s => s.TransactionHash);
            }

            return queryBlock;

        }

        public QueryBlock GetLastBlock(bool getTransactions = true)
        {
            var block = this.storage.BlockGetCompleteBlockCount(1).FirstOrDefault();

            if (block == null)
            {
                return new QueryBlock();
            }

            var queryBlock = new QueryBlock
            {
                CoinTag = this.configuration.CoinTag,
                BlockHash = block.BlockHash,
                BlockIndex = block.BlockIndex,
                BlockSize = block.BlockSize,
                BlockTime = block.BlockTime,
                NextBlockHash = block.NextBlockHash,
                PreviousBlockHash = block.PreviousBlockHash,
                Synced = block.SyncComplete,
                TransactionCount = block.TransactionCount,
                Bits = block.Bits,
                Confirmations = block.Confirmations,
                Merkleroot = block.Merkleroot,
                Nonce = block.Nonce,
                PosBlockSignature = block.PosBlockSignature,
                PosBlockTrust = block.PosBlockTrust,
                PosChainTrust = block.PosChainTrust,
                PosFlags = block.PosFlags,
                PosHashProof = block.PosHashProof,
                PosModifierv2 = block.PosModifierv2,
                Version = block.Version,
                Transactions = Enumerable.Empty<string>()
            };

            if (getTransactions)
            {
                var transactions = this.storage.BlockTransactionGetByBlockIndex(block.BlockIndex);
                queryBlock.Transactions = transactions.Select(s => s.TransactionHash);
            }

            return queryBlock;
        }

        public QueryTransaction GetTransaction(string transactionId)
        {
            var transaction = this.storage.BlockTransactionGet(transactionId);

            if (transaction == null)
            {
                return new QueryTransaction();
            }

            var transactionItems = this.storage.TransactionItemsGet(transactionId);

            return new QueryTransaction
            {
                CoinTag = this.configuration.CoinTag,
                BlockHash = transaction.BlockHash,
                BlockIndex = transaction.BlockIndex,
                Timestamp = transaction.Timestamp.UnixTimeStampToDateTime(),
                TransactionId = transaction.TransactionHash,
                Confirmations = transaction.Confirmations,
                Outputs = transactionItems.Outputs.Select(o => new QueryTransactionOutput
                {
                    Address = o.Address,
                    Balance = o.Value,
                    Index = o.Index,
                    OutputType = o.OutputType
                }),
                Inputs = transactionItems.Inputs.Select(i => new QueryTransactionInput
                {
                    CoinBase = i.InputCoinBase,
                    InputAddress = string.Empty,
                    InputIndex = i.PreviousIndex,
                    InputTransactionId = i.PreviousTransactionHash
                })
            };
        }
    }
}
