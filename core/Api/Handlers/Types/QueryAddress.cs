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
    using System.Collections.Generic;

    public class QueryAddress
    {
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
        public long Balance { get; set; }

        /// <summary>
        /// Gets or sets the total received.
        /// </summary>
        public long? TotalReceived { get; set; }

        /// <summary>
        /// Gets or sets the total sent.
        /// </summary>
        public long? TotalSent { get; set; }

        /// <summary>
        /// Gets or sets the unconfirmed balance.
        /// </summary>
        public long UnconfirmedBalance { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        public IEnumerable<QueryAddressItem> Transactions { get; set; }

        /// <summary>
        /// Gets or sets the Unconfirmed transactions.
        /// </summary>
        public IEnumerable<QueryAddressItem> UnconfirmedTransactions { get; set; }
    }
}
