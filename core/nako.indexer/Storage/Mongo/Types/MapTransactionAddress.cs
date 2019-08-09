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
    using System.Collections.Generic;

    public class MapTransactionAddress
    {
        public string Id { get; set; }

        public int Index { get; set; }

        public List<string> Addresses { get; set; }

        public string TransactionId { get; set; }

        public string ScriptHex { get; set; }

        public long Value { get; set; }

        public string SpendingTransactionId { get; set; }

        public long? SpendingBlockIndex { get; set; }

        public long BlockIndex { get; set; }

        public bool CoinBase { get; set; }

        public bool CoinStake { get; set; }
    }
}