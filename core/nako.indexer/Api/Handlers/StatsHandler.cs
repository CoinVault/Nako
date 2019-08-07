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
                stats.ClientInfo = await client.GetInfoAsync();
            }
            catch (Exception ex)
            {
                stats.ClientInfo = new ClientInfo { Errors = ex.Message };
            }

            stats.TransactionsInPool = this.storage.GetMemoryTransactions().Count();

            try
            {
                stats.SyncBlockIndex = this.storage.BlockGetBlockCount(1).First().BlockIndex;
                stats.Progress = $"{stats.SyncBlockIndex}/{stats.ClientInfo.Blocks} - {stats.ClientInfo.Blocks - stats.SyncBlockIndex}";
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
                var ipe = CreateIPEndPoint(p.Addr);
                p.Addr = $"{ipe.Address}:{ipe.Port}";
            });

            return res;
        }

        /// <summary>
        /// Handles IPv4 and IPv6 notation.
        /// </summary>
        public static IPEndPoint CreateIPEndPoint(string endPoint)
        {
            string[] ep = endPoint.Split(':');
            if (ep.Length < 2) throw new FormatException("Invalid endpoint format");
            IPAddress ip;
            if (ep.Length > 2)
            {
                if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            else
            {
                if (!IPAddress.TryParse(ep[0], out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            int port;
            if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }
            return new IPEndPoint(ip, port);
        }
    }
}
