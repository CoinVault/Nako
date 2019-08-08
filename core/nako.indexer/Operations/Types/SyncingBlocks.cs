// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncingBlocks.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Operations.Types
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Nako.Client.Types;

    public class SyncingBlocks
    {
        public ConcurrentDictionary<string, BlockInfo> CurrentSyncing { get; set; }

        public BlockInfo LastBlock { get; set; }

        public long LastClientBlockIndex { get; set; }

        public bool Blocked { get; set; }

        public List<string> CurrentPoolSyncing { get; set; }
    }
}
