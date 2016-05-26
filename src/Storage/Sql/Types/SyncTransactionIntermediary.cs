// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncTransactionIntermediary.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage.Sql.Types
{
    #region Using Directives

    using System;

    using Nako.Storage.Types;

    #endregion

    /// <summary>
    /// NOTE:The sql functionality is not maintained.
    /// </summary>
    public class SyncTransactionIntermediary
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the input index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the input index.
        /// </summary>
        public SyncTransactionIndexType IndexType { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        public string InputAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this transaction is the first transaction 'coin-base'.
        /// </summary>
        public string InputCoinBase { get; set; }

        /// <summary>
        /// Gets or sets the input index.
        /// </summary>
        public int? InputIndex { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public string InputTransactionHash { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public decimal? InputValue { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        public string OutputAddress { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public string OutputType { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public decimal? OutputValue { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public bool? OutputSpent { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public string OutputSpentTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the Timestamp.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public string TransactionId { get; set; }

        #endregion
    }
}