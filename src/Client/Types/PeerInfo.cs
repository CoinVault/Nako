// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PeerInfo.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Nako.Client.Types
{
    #region Using Directives

    using System.Collections.Generic;

    using Newtonsoft.Json;

    #endregion

    public class PeerInfo
    {
        public uint Id { get; set; }

        public string Addr { get; set; }

        public string AddrLocal { get; set; }

        public string Services { get; set; }

        public int LastSend { get; set; }

        public int LastRecv { get; set; }

        public int BytesSent { get; set; }

        public int BytesRecv { get; set; }

        public int ConnTime { get; set; }

        public double PingTime { get; set; }

        public double Version { get; set; }

        public string SubVer { get; set; }

        public bool Inbound { get; set; }

        public int StartingHeight { get; set; }

        public int BanScore { get; set; }

        [JsonProperty("synced_headers")]
        public int SyncedHeaders { get; set; }

        [JsonProperty("synced_blocks")]
        public int SyncedBlocks { get; set; }

        public IList<int> InFlight { get; set; }

        public bool WhiteListed { get; set; }
    }
}