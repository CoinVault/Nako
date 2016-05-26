// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncTransactionAddressBalance.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage.Types
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The query address.
    /// </summary>
    public class SyncTransactionAddressBalance
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the total received.
        /// </summary>
        public decimal Available { get; set; }

        /// <summary>
        /// Gets or sets the total received.
        /// </summary>
        public decimal? Received { get; set; }

        /// <summary>
        /// Gets or sets the total sent.
        /// </summary>
        public decimal? Sent { get; set; }

        /// <summary>
        /// Gets or sets the unconfirmed balance.
        /// </summary>
        public decimal Unconfirmed { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public IEnumerable<SyncTransactionAddressItem> Items { get; set; }

        #endregion
    }
}
