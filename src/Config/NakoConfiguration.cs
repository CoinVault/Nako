// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NakoConfiguration.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Config
{
    /// <summary>
    /// The sync configuration.
    /// </summary>
    public class NakoConfiguration
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        public int BlockStoreInterval { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        public int BlockSyncerInterval { get; set; }

        /// <summary>
        /// Gets or sets the coin tag.
        /// </summary>
        public string ClientLocation { get; set; }

        /// <summary>
        /// Gets or sets the coin tag.
        /// </summary>
        public string CoinTag { get; set; }

        /// <summary>
        /// Gets or sets the RpcPassword.
        /// </summary>
        public string RpcPassword { get; set; }

        /// <summary>
        /// Gets or sets the pool.
        /// </summary>
        public int BlockFinderInterval { get; set; }

        /// <summary>
        /// Gets or sets the parallel block sync stop count.
        /// </summary>
        public int DetailedTrace { get; set; }

        /// <summary>
        /// Gets or sets the RpcUser.
        /// </summary>
        public int MaxItemsInQueue { get; set; }

        /// <summary>
        /// Gets or sets the User.
        /// </summary>
        public int ParallelRequestsToTransactionRpc { get; set; }

        /// <summary>
        /// Gets or sets the RpcUser.
        /// </summary>
        public int SyncApiPort { get; set; }

        /// <summary>
        /// Gets or sets the RpcUser.
        /// </summary>
        public int ParalleleTableStorageBatchCount { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        public int RpcAccessPort { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether secure.
        /// </summary>
        public bool RpcSecure { get; set; }

        /// <summary>
        /// Gets or sets the coin tag.
        /// </summary>
        public string RpcDomain { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether secure.
        /// </summary>
        public bool SyncBlockchain { get; set; }

        /// <summary>
        /// Gets or sets the sync first block index.
        /// </summary>
        public long StartBlockIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether secure.
        /// </summary>
        public bool SyncMemoryPool { get; set; }

        /// <summary>
        /// Gets or sets the RpcUser.
        /// </summary>
        public string RpcUser { get; set; }

        /// <summary>
        /// Gets or sets the Notify Url.
        /// </summary>
        public string NotifyUrl { get; set; }

        /// <summary>
        /// Gets or sets the Notify Url.
        /// </summary>
        public int NotifyBatchCount { get; set; }

        #endregion
    }
}
