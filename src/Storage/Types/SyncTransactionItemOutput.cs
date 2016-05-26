// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncTransactionItemOutput.cs" company="SoftChains">
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
    public class SyncTransactionItemOutput
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
        /// Gets or sets the amount.
        /// </summary>
        public string OutputType { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public decimal Value { get; set; }

        #endregion
    }
}