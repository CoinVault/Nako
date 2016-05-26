// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryAddress.cs" company="SoftChains">
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

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The query address.
    /// </summary>
    public class QueryAddress
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the CoinTag.
        /// </summary>
        public string CoinTag { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the total received.
        /// </summary>
        public decimal? TotalReceived { get; set; }

        /// <summary>
        /// Gets or sets the total sent.
        /// </summary>
        public decimal? TotalSent { get; set; }

        /// <summary>
        /// Gets or sets the unconfirmed balance.
        /// </summary>
        public decimal UnconfirmedBalance { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        public IEnumerable<QueryAddressItem> Transactions { get; set; }

        /// <summary>
        /// Gets or sets the Unconfirmed transactions.
        /// </summary>
        public IEnumerable<QueryAddressItem> UnconfirmedTransactions { get; set; }

        #endregion
    }
}
