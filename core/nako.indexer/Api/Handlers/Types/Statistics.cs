// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Statistics.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Api.Handlers.Types
{
    using Nako.Client.Types;

    public class CoinInfo
    {
        public string CoinTag { get; set; }

        public long BlockHeight { get; set; }

        public string LogoUrl { get; set; }

        public string CoinName { get; set; }

        public string CoinInformation { get; set; }
    }

    public class Statistics
    {
        /// <summary>
        /// Gets or sets the coin tag.
        /// </summary>
        public string CoinTag { get; set; }

        /// <summary>
        /// Gets or sets the sync progress.
        /// </summary>
        public string Progress { get; set; }

        /// <summary>
        /// Gets or sets the number of transactions in pool.
        /// </summary>
        public int TransactionsInPool { get; set; }

        /// <summary>
        /// Gets or sets the current block index.
        /// </summary>
        public long SyncBlockIndex { get; set; } 

        /// <summary>
        /// Gets or sets some data bout a coin.
        /// </summary>
        public ClientInfo ClientInfo { get; set; }

        /// <summary>
        /// Gets or sets the blocks per minute being processed.
        /// </summary>
        public string BlocksPerMinute { get; set; }
    }
}
