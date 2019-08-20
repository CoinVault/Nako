// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InsertStats.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Operations.Types
{
    #region Using Directives

    using System.Collections.Generic;

    using Nako.Storage.Mongo.Types;

    #endregion

    /// <summary>
    /// The insert stats.
    /// </summary>
    public class InsertStats
    {
        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        public int Transactions { get; set; }

        public int RawTransactions { get; set; }

        /// <summary>
        /// Gets or sets the outputs.
        /// </summary>
        public int InputsOutputs { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public List<MapTransactionAddress> Items { get; set; }
    }
}