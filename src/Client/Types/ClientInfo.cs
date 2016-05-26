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
    /// <summary>
    /// The client info.
    /// </summary>
    public class ClientInfo
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the protocol version.
        /// </summary>
        public string ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets the wallet version.
        /// </summary>
        public string WalletVersion { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Gets or sets the blocks.
        /// </summary>
        public long Blocks { get; set; }

        /// <summary>
        /// Gets or sets the time offset.
        /// </summary>
        public double TimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the connections.
        /// </summary>
        public long Connections { get; set; }

        /// <summary>
        /// Gets or sets the proxy.
        /// </summary>
        public string Proxy { get; set; }

        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        public double Difficulty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether test net.
        /// </summary>
        public bool Testnet { get; set; }

        /// <summary>
        /// Gets or sets the key pool eldest.
        /// </summary>
        public long KeyPoolEldest { get; set; }

        /// <summary>
        /// Gets or sets the key pool size.
        /// </summary>
        public long KeyPoolSize { get; set; }

        /// <summary>
        /// Gets or sets the pay fee.
        /// </summary>
        public decimal PayTxFee { get; set; }

        /// <summary>
        /// Gets or sets the relay fee.
        /// </summary>
        public decimal RelayTxFee { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public string Errors { get; set; }
    }
}