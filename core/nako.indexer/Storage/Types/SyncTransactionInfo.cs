// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncTransactionInfo.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Nako.Storage.Types
{
    public class SyncTransactionInfo
    {
        public string BlockHash { get; set; }

        public long BlockIndex { get; set; }

        public long Timestamp { get; set; }

        public string TransactionHash { get; set; }

        public long Confirmations { get; set; }
    }
}