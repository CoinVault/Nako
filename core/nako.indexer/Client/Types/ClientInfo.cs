// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientInfo.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Client.Types
{
    public class NetworkInfoModel
    {
        //[JsonProperty(PropertyName = "version")]
        public uint Version { get; set; }

        //[JsonProperty(PropertyName = "subversion")]
        public string SubVersion { get; set; }

        //[JsonProperty(PropertyName = "protocolversion")]
        public uint ProtocolVersion { get; set; }

        //[JsonProperty(PropertyName = "localservices")]
        public string LocalServices { get; set; }

        //[JsonProperty(PropertyName = "localrelay")]
        public bool IsLocalRelay { get; set; }

        //[JsonProperty(PropertyName = "timeoffset")]
        public long TimeOffset { get; set; }

        //[JsonProperty(PropertyName = "connections", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Connections { get; set; }

        //[JsonProperty(PropertyName = "networkactive")]
        public bool IsNetworkActive { get; set; }

        //[JsonProperty(PropertyName = "relayfee")]
        public decimal RelayFee { get; set; }

        //[JsonProperty(PropertyName = "incrementalfee")]
        public decimal IncrementalFee { get; set; }
    }

    public class BlockchainInfoModel
    {
        //[JsonProperty(PropertyName = "chain")]
        public string Chain { get; set; }

        //[JsonProperty(PropertyName = "blocks")]
        public uint Blocks { get; set; }

        //[JsonProperty(PropertyName = "headers")]
        public uint Headers { get; set; }

        //[JsonProperty(PropertyName = "bestblockhash")]
        public string BestBlockHash { get; set; }

        //[JsonProperty(PropertyName = "difficulty")]
        public double Difficulty { get; set; }

        //[JsonProperty(PropertyName = "mediantime")]
        public long MedianTime { get; set; }

        //[JsonProperty(PropertyName = "verificationprogress")]
        public double VerificationProgress { get; set; }

        //[JsonProperty(PropertyName = "initialblockdownload")]
        public bool IsInitialBlockDownload { get; set; }

        //[JsonProperty(PropertyName = "chainwork")]
        public string Chainwork { get; set; }

        //[JsonProperty(PropertyName = "pruned")]
        public bool IsPruned { get; set; }
    }

    public class ClientInfo
    {
        public string Version { get; set; }

        public string ProtocolVersion { get; set; }

        public string WalletVersion { get; set; }

        public decimal Balance { get; set; }

        public long Blocks { get; set; }

        public double TimeOffset { get; set; }

        public long Connections { get; set; }

        public string Proxy { get; set; }

        //public double Difficulty { get; set; }

        public bool Testnet { get; set; }

        public long KeyPoolEldest { get; set; }

        public long KeyPoolSize { get; set; }

        public decimal PayTxFee { get; set; }

        public decimal RelayTxFee { get; set; }

        public string Errors { get; set; }
    }
}