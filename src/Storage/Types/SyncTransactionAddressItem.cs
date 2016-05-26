// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncTransactionAddressItem.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage.Types
{
    /// <summary>
    /// The tracking info.
    /// </summary>
    public class SyncTransactionAddressItem
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the input index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the transaction hash.
        /// </summary>
        public string TransactionHash { get; set; }

        /// <summary>
        /// Gets or sets the spending transaction hash.
        /// </summary>
        public string SpendingTransactionHash { get; set; }

        /// <summary>
        /// Gets or sets the script public key hex.
        /// </summary>
        public string ScriptHex { get; set; }

        /// <summary>
        /// Gets or sets the coin base.
        /// </summary>
        public string CoinBase { get; set; }

        /// <summary>
        /// Gets or sets the coin base.
        /// </summary>
        public long? BlockIndex { get; set; }

        /// <summary>
        /// Gets or sets the coin base.
        /// </summary>
        public long? Confirmations { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the block Time.
        /// </summary>
        public long Time { get; set; }

        #endregion
    }
}