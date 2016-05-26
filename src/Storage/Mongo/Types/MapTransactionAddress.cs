// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MapTransactionAddress.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage.Mongo.Types
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The tracking info.
    /// </summary>
    public class MapTransactionAddress
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the input index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        public List<string> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the script public key hex.
        /// </summary>
        public string ScriptHex { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// Ideally we'd use a decimal but, Mongo doesn't support decimal types and will default to string. 
        /// String types can not be used in a sum operation so we'll represent the value as a double.
        /// We'll convert the double to decimal on the mapping layer.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public string SpendingTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the block Height.
        /// </summary>
        public long BlockIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether coin base.
        /// </summary>
        public bool CoinBase { get; set; }

        #endregion
    }
}