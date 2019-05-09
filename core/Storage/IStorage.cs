// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorage.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage
{
    using System.Collections.Generic;
    using Nako.Client.Types;
    using Nako.Storage.Types;

    public interface IStorage
    {
        IEnumerable<SyncBlockInfo> BlockGetIncompleteBlocks();

        IEnumerable<SyncBlockInfo> BlockGetBlockCount(int count);

        IEnumerable<SyncBlockInfo> BlockGetCompleteBlockCount(int count);

        SyncBlockInfo BlockGetByHash(string blockHash);

        SyncBlockInfo BlockGetByIndex(long blockIndex);

        SyncTransactionInfo BlockTransactionGet(string transactionId);

        IEnumerable<SyncTransactionInfo> BlockTransactionGetByBlock(string blockHash);

        IEnumerable<SyncTransactionInfo> BlockTransactionGetByBlockIndex(long blockIndex);

        SyncTransactionItemOutput TransactionsGet(string transactionId, int index, SyncTransactionIndexType indexType);

        SyncTransactionItems TransactionItemsGet(string transactionId);

        SyncTransactionAddressBalance AddressGetBalance(string address, long confirmations);

        SyncTransactionAddressBalance AddressGetBalanceUtxo(string address, long confirmations);

        void DeleteBlock(string blockHash);

        IEnumerable<DecodedRawTransaction> GetMemoryTransactions();
    }
}

