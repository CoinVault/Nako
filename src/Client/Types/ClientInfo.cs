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

        public double Difficulty { get; set; }

        public bool Testnet { get; set; }

        public long KeyPoolEldest { get; set; }

        public long KeyPoolSize { get; set; }

        public decimal PayTxFee { get; set; }

        public decimal RelayTxFee { get; set; }

        public string Errors { get; set; }
    }
}