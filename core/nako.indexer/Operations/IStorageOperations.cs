﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorageOperations.cs" company="SoftChains">
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

    using Nako.Operations.Types;

    #endregion

    /// <summary>
    /// The StorageOperations interface.
    /// </summary>
    public interface IStorageOperations
    {
        /// <summary>
        /// Validate a block.
        /// </summary>
        void ValidateBlock(SyncBlockTransactionsOperation item);

        /// <summary>
        /// Insert transactions.
        /// </summary>
        InsertStats InsertTransactions(SyncBlockTransactionsOperation item);
    }
}