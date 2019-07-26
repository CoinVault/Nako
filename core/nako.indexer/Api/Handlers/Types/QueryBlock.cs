// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryBlock.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Api.Handlers.Types
{
    using System.Collections.Generic;

    public class QueryBlock
    {
        /// <summary>
        /// Gets or sets the CoinTag.
        /// </summary>
        public string CoinTag { get; set; }

        /// <summary>
        /// Gets or sets the block hash.
        /// </summary>
        public string BlockHash { get; set; }

        /// <summary>
        /// Gets or sets the block Height.
        /// </summary>
        public long BlockIndex { get; set; }

        /// <summary>
        /// Gets or sets the block Size.
        /// </summary>
        public long BlockSize { get; set; }

        /// <summary>
        /// Gets or sets the block Time.
        /// </summary>
        public long BlockTime { get; set; }

        /// <summary>
        /// Gets or sets the block NextHash.
        /// </summary>
        public string NextBlockHash { get; set; }

        /// <summary>
        /// Gets or sets the block PreviousHash.
        /// </summary>
        public string PreviousBlockHash { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether sync is complete for this block.
        /// </summary>
        public bool Synced { get; set; }

        /// <summary>
        /// Gets or sets the Transaction Count.
        /// </summary>
        public int TransactionCount { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        public IEnumerable<string> Transactions { get; set; }
    }
}
