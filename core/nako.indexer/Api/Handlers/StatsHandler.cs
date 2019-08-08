// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatsHandler.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Api.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Nako.Api.Handlers.Types;
    using Nako.Client;
    using Nako.Client.Types;
    using Nako.Config;
    using Nako.Operations.Types;
    using Nako.Storage;
    using System.Globalization;
    using System.Net;

    /// <summary>
    /// Handler to make get info about a blockchain.
    /// </summary>
    public class StatsHandler 
    {
        private readonly SyncConnection syncConnection;

        private readonly IStorage storage;

        private readonly NakoConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsHandler"/> class.
        /// </summary>
        public StatsHandler(SyncConnection connection, IStorage storage, IOptions<NakoConfiguration> configuration)
        {
            this.storage = storage;
            this.syncConnection = connection;
            this.configuration = configuration.Value;
        }

        public async Task<StatsConnection> StatsConnection()
        {
            var connection = this.syncConnection;
            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            var clientConnection = await client.GetConnectionCountAsync();
            return new StatsConnection { Connections = clientConnection };
        }

        public async Task<CoinInfo> CoinInformation()
        {
            var index = this.storage.BlockGetBlockCount(1).FirstOrDefault()?.BlockIndex ?? 0;

            return new CoinInfo
            {
                CoinTag = this.configuration.CoinTag,
                BlockHeight = index,
                LogoUrl = this.configuration.LogoUrl,
                CoinName = this.configuration.CoinName,
                CoinInformation = this.configuration.CoinInformation
            };
        }

        public async Task<Statistics> Statistics()
        {
            var connection = this.syncConnection;
            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);
            var stats = new Statistics { CoinTag = this.syncConnection.CoinTag };

            try
            {
                stats.BlockchainInfo = await client.GetBlockchainInfo();
                stats.NetworkInfo = await client.GetNetworkInfo();
            }
            catch (Exception ex)
            {
                stats.Error = ex.Message;
                return stats;   
            }

            stats.TransactionsInPool = this.storage.GetMemoryTransactions().Count();

            try
            {
                stats.SyncBlockIndex = this.storage.BlockGetBlockCount(1).First().BlockIndex;
                stats.Progress = $"{stats.SyncBlockIndex}/{stats.BlockchainInfo.Blocks} - {stats.BlockchainInfo.Blocks - stats.SyncBlockIndex}";
            }
            catch (Exception ex)
            {
                stats.Progress = ex.Message;
            }

            return stats;
        }

        public async Task<List<PeerInfo>> Peers()
        {
            var connection = this.syncConnection;
            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);
            var res = (await client.GetPeerInfo()).ToList();

            res.ForEach(p =>
            {
                if (TryParse(p.Addr, out IPEndPoint ipe))
                {
                    string addr = ipe.Address.ToString();
                    if (ipe.Address.IsIPv4MappedToIPv6)
                    {
                        addr = ipe.Address.MapToIPv4().ToString();
                    }

                    p.Addr = $"{addr}:{ipe.Port}";
                }
            });

            return res;
        }


        // This code is temporary til Nako upgrades to netcore 3.3
        // see https://github.com/dotnet/corefx/pull/33119
        public const int MaxPort = 0x0000FFFF;

        public static bool TryParse(string s, out IPEndPoint result)
        {
            return TryParse(s.AsSpan(), out result);
        }


        public static bool TryParse(ReadOnlySpan<char> s, out IPEndPoint result)
        {
            int addressLength = s.Length;  // If there's no port then send the entire string to the address parser
            int lastColonPos = s.LastIndexOf(':');

            // Look to see if this is an IPv6 address with a port.
            if (lastColonPos > 0)
            {
                if (s[lastColonPos - 1] == ']')
                {
                    addressLength = lastColonPos;
                }
                // Look to see if this is IPv4 with a port (IPv6 will have another colon)
                else if (s.Slice(0, lastColonPos).LastIndexOf(':') == -1)
                {
                    addressLength = lastColonPos;
                }
            }

            if (IPAddress.TryParse(s.Slice(0, addressLength), out IPAddress address))
            {
                uint port = 0;
                if (addressLength == s.Length ||
                    (uint.TryParse(s.Slice(addressLength + 1), NumberStyles.None, CultureInfo.InvariantCulture, out port) && port <= MaxPort))

                {
                    result = new IPEndPoint(address, (int)port);
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}
