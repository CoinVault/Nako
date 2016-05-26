// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MapTransactionBlock.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage.Mongo.Types
{
    /// <summary>
    /// The tracking info.
    /// </summary>
    public class MapTransactionBlock
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        public long BlockIndex { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public string TransactionId { get; set; }

        #endregion
    }
}