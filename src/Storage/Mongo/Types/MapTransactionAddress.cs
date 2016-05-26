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
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    public class MapTransactionAddress
    {
        #region Public Properties

        public string Id { get; set; }

        public int Index { get; set; }

        public List<string> Addresses { get; set; }

        public string TransactionId { get; set; }

        public string ScriptHex { get; set; }

        public double Value { get; set; }

        public string SpendingTransactionId { get; set; }

        public long BlockIndex { get; set; }

        public bool CoinBase { get; set; }

        #endregion
    }
}