// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncTransactionInfo.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage.Types
{
    /// <summary>
    /// The tracking info.
    /// </summary>
    public class SyncTransactionInfo
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the block hash.
        /// </summary>
        public string BlockHash { get; set; }

        /// <summary>
        /// Gets or sets the block index.
        /// </summary>
        public long BlockIndex { get; set; }

        /// <summary>
        /// Gets or sets the Timestamp.
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public string TransactionHash { get; set; }

        /// <summary>
        /// Gets or sets the coin base.
        /// </summary>
        public long Confirmations { get; set; }

        #endregion
    }
}