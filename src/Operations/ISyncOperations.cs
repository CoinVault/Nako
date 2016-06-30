// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISyncOperations.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Operations
{
    #region Using Directives

    using System.Threading.Tasks;

    using Nako.Client.Types;
    using Nako.Operations.Types;

    #endregion

    /// <summary>
    /// The SyncOperations interface.
    /// </summary>
    public interface ISyncOperations
    {
        /// <summary>
        /// The sync block.
        /// </summary>
        SyncBlockOperation FindBlock(SyncConnection connection, SyncingBlocks container);

        /// <summary>
        /// The sync block.
        /// </summary>
        SyncPoolTransactions FindPoolTransactions(SyncConnection connection, SyncingBlocks container);

        /// <summary>
        /// The sync memory pool.
        /// </summary>
        SyncBlockTransactionsOperation SyncPool(SyncConnection connection, SyncPoolTransactions poolTransactions);

        /// <summary>
        /// The sync transactions.
        /// </summary>
        SyncBlockTransactionsOperation SyncBlock(SyncConnection connection, BlockInfo block);

        /// <summary>
        /// The check block reorganization.
        /// </summary>
        Task CheckBlockReorganization(SyncConnection connection);
    }
}