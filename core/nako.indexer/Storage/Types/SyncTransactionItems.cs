// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncTransactionItems.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Nako.Storage.Types
{
    using System.Collections.Generic;

    public class SyncTransactionItems
    {
        public bool IsCoinbase { get; set; }

        public bool IsCoinstake { get; set; }

        public string LockTime { get; set; }

        public bool RBF { get; set; }

        public uint Version { get; set; }

        public List<SyncTransactionItemInput> Inputs { get; set; }

        public List<SyncTransactionItemOutput> Outputs { get; set; }
    }
}