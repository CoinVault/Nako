// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DecodedRawTransaction.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Client.Types
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The decoded raw transaction.
    /// </summary>
    public class DecodedRawTransaction
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the hex.
        /// </summary>
        public string Hex { get; set; }

        /// <summary>
        /// Gets or sets the lock time.
        /// </summary>
        public long Locktime { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public string TxId { get; set; }

        /// <summary>
        /// Gets or sets the v in.
        /// </summary>
        public List<Vin> VIn { get; set; }

        /// <summary>
        /// Gets or sets the v out.
        /// </summary>
        public List<Vout> VOut { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public int Version { get; set; }

        #endregion
    }
}
