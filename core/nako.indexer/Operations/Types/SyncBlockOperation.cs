// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncBlockOperation.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Operations.Types
{
    #region Using Directives

    using Nako.Client.Types;

    #endregion

    /// <summary>
    /// The sync block info.
    /// </summary>
    public class SyncBlockOperation
    {
        /// <summary>
        /// Gets or sets the block info.
        /// </summary>
        public BlockInfo BlockInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether incomplete block.
        /// </summary>
        public bool IncompleteBlock { get; set; }

        /// <summary>
        /// Gets or sets the last crypto block index.
        /// </summary>
        public long LastCryptoBlockIndex { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        public SyncPoolTransactions PoolTransactions { get; set; }
    }
}
