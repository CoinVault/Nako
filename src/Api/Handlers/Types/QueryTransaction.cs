// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryTransaction.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Api.Handlers.Types
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The query transaction.
    /// </summary>
    public class QueryTransaction
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the CoinTag.
        /// </summary>
        public string CoinTag { get; set; }

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
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the coin base.
        /// </summary>
        public long Confirmations { get; set; }

        /// <summary>
        /// Gets or sets the block.
        /// </summary>
        public IEnumerable<QueryTransactionInput> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the block.
        /// </summary>
        public IEnumerable<QueryTransactionOutput> Outputs { get; set; }

        #endregion
    }
}
