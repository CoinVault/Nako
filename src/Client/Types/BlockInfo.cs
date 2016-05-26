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
    #region Using Directives

    using System.Collections.Generic;

    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// The block info.
    /// </summary>
    public class BlockInfo
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the bits.
        /// </summary>
        [JsonProperty("bits")]
        public string Bits { get; set; }

        /// <summary>
        /// Gets or sets the confirmations.
        /// </summary>
        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }

        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        [JsonProperty("difficulty")]
        public string Difficulty { get; set; }

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        [JsonProperty("hash")]
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [JsonProperty("height")]
        public long Height { get; set; }

        /// <summary>
        /// Gets or sets the root.
        /// </summary>
        [JsonProperty("merkleroot")]
        public string Merkleroot { get; set; }

        /// <summary>
        /// Gets or sets the next block hash.
        /// </summary>
        [JsonProperty("nextblockhash")]
        public string NextBlockHash { get; set; }

        /// <summary>
        /// Gets or sets the nonce.
        /// </summary>
        [JsonProperty("nonce")]
        public long Nonce { get; set; }

        /// <summary>
        /// Gets or sets the previous block hash.
        /// </summary>
        [JsonProperty("previousblockhash")]
        public string PreviousBlockHash { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        [JsonProperty("size")]
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(JsonUnixTimeConverter))]
        public long Time { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        [JsonProperty("tx")]
        public IEnumerable<string> Transactions { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        [JsonProperty("version")]
        public long Version { get; set; }

        #endregion
    }
}