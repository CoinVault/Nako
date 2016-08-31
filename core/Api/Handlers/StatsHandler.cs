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
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Nako.Api.Handlers.Types;
    using Nako.Client;
    using Nako.Client.Types;
    using Nako.Operations.Types;
    using Nako.Storage;

    #endregion

    /// <summary>
    /// Handler to make get info about a blockchain.
    /// </summary>
    public class StatsHandler 
    {
        private readonly SyncConnection syncConnection;

        private readonly IStorage storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsHandler"/> class.
        /// </summary>
        public StatsHandler(SyncConnection connection, IStorage storage)
        {
            this.storage = storage;
            this.syncConnection = connection;
        }

        #region Public Methods and Operators

        public async Task<StatsConnection> StatsConnection()
        {
            var connection = this.syncConnection;
            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            var clientConnection = await client.GetConnectionCountAsync();
            return new StatsConnection { Connections = clientConnection };
        }

        public async Task<Statistics> Statistics()
        {
            var connection = this.syncConnection;
            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);
            
            var stats = new Statistics { CoinTag = this.syncConnection.CoinTag };

            stats.ClientInfo = await client.GetInfoAsync();
            stats.TransactionsInPool = this.storage.GetMemoryTransactions().Count();
            stats.SyncBlockIndex = this.storage.BlockGetBlockCount(1).First().BlockIndex;
            stats.Progress = $"{stats.SyncBlockIndex}/{stats.ClientInfo.Blocks} - {stats.ClientInfo.Blocks - stats.SyncBlockIndex}";
            return stats;
        }

        public async Task<List<PeerInfo>> Peers()
        {
            var connection = this.syncConnection;
            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);
            return (await client.GetPeerInfo()).ToList();
        }

        #endregion
    }
}
