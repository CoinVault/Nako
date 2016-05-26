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

    /// <summary>
    /// The client info.
    /// </summary>
    public class PeerInfo
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Gets or sets the.
        /// </summary>
        public string Addr { get; set; }

        /// <summary>
        /// Gets or sets the local.
        /// </summary>
        public string AddrLocal { get; set; }

        /// <summary>
        /// Gets or sets the services.
        /// </summary>
        public string Services { get; set; }

        /// <summary>
        /// Gets or sets the last send.
        /// </summary>
        public int LastSend { get; set; }

        /// <summary>
        /// Gets or sets the last.
        /// </summary>
        public int LastRecv { get; set; }

        /// <summary>
        /// Gets or sets the bytes sent.
        /// </summary>
        public int BytesSent { get; set; }

        /// <summary>
        /// Gets or sets the bytes.
        /// </summary>
        public int BytesRecv { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        public int ConnTime { get; set; }

        /// <summary>
        /// Gets or sets the ping time.
        /// </summary>
        public double PingTime { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public double Version { get; set; }

        /// <summary>
        /// Gets or sets the sub.
        /// </summary>
        public string SubVer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether inbound.
        /// </summary>
        public bool Inbound { get; set; }

        /// <summary>
        /// Gets or sets the starting height.
        /// </summary>
        public int StartingHeight { get; set; }

        /// <summary>
        /// Gets or sets the ban score.
        /// </summary>
        public int BanScore { get; set; }

        /// <summary>
        /// Gets or sets the synced headers.
        /// </summary>
        [JsonProperty("synced_headers")]
        public int SyncedHeaders { get; set; }

        /// <summary>
        /// Gets or sets the synced blocks.
        /// </summary>
        [JsonProperty("synced_blocks")]
        public int SyncedBlocks { get; set; }

        /// <summary>
        /// Gets or sets the in flight.
        /// </summary>
        public IList<int> InFlight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether white listed.
        /// </summary>
        public bool WhiteListed { get; set; }
    }
}