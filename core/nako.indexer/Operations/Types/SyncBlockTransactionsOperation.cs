// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncBlockTransactionsOperation.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using NBitcoin;

namespace Nako.Operations.Types
{
    using System.Collections.Generic;

    using Nako.Client.Types;

    /// <summary>
    /// The sync block info.
    /// </summary>
    public class SyncBlockTransactionsOperation
    {
        /// <summary>
        /// Gets or sets the block info.
        /// </summary>
        public BlockInfo BlockInfo { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
