// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockInfo.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Client.Types
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class BlockInfo
    {
        [JsonProperty("bits")]
        public string Bits { get; set; }

        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }

        //[JsonProperty("difficulty")]
        //public string Difficulty { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("merkleroot")]
        public string Merkleroot { get; set; }

        [JsonProperty("nextblockhash")]
        public string NextBlockHash { get; set; }

        [JsonProperty("nonce")]
        public long Nonce { get; set; }

        [JsonProperty("previousblockhash")]
        public string PreviousBlockHash { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("time")]
        [JsonConverter(typeof(JsonUnixTimeConverter))]
        public long Time { get; set; }

        [JsonProperty("tx")]
        public IEnumerable<string> Transactions { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("signature")]
        public string PosBlockSignature { get; set; }

        [JsonProperty("modifierv2")]
        public string PosModifierv2 { get; set; }

        [JsonProperty("flags")]
        public string PosFlags { get; set; }

        [JsonProperty("hashproof")]
        public string PosHashProof { get; set; }

        [JsonProperty("blocktrust")]
        public string PosBlockTrust { get; set; }

        [JsonProperty("chaintrust")]
        public string PosChainTrust { get; set; }
    }
}