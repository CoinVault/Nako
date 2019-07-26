﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MapBlock.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Nako.Storage.Mongo.Types
{
    public class MapBlock
    {
        #region Public Properties

        public string BlockHash { get; set; }

        public long BlockIndex { get; set; }

        public long BlockSize { get; set; }

        public long BlockTime { get; set; }

        public string NextBlockHash { get; set; }

        public string PreviousBlockHash { get; set; }

        public bool SyncComplete { get; set; }

        public int TransactionCount { get; set; }

        #endregion
    }
}