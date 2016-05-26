// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Concurrency.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage.Sql
{
    #region Using Directives

    using System.Collections.Concurrent;

    using Nako.Storage.Types;

    #endregion

    /// <summary>
    /// The concurrency.
    /// </summary>
    public class Concurrency
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the collection.
        /// </summary>
        public static ConcurrentBag<SyncTransactionItemOutput> TransactionCollection { get; set; }

        /// <summary>
        /// Gets or sets the collection.
        /// </summary>
        public static ConcurrentBag<SyncTransactionInfo> BlockTransactionCollection { get; set; }
       
        #endregion
    }
}
