// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryTransactionInput.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Api.Handlers.Types
{
    /// <summary>
    /// The query transaction.
    /// </summary>
    public class QueryTransactionInput
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the input index.
        /// </summary>
        public int InputIndex { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        public string InputAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this transaction is the first transaction 'coin-base'.
        /// </summary>
        public string CoinBase { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public string InputTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public decimal? InputValue { get; set; }

        #endregion
    }
}
