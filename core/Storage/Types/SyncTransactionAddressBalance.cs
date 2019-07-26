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

    public class SyncTransactionAddressBalance
    {
        #region Public Properties

        public long Available { get; set; }

        public long? Received { get; set; }

        public long? Sent { get; set; }

        public long Unconfirmed { get; set; }

        public IEnumerable<SyncTransactionAddressItem> Items { get; set; }

        #endregion
    }
}
