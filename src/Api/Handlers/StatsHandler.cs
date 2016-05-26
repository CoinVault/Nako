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
    /// The CoinOperations interface.
    /// </summary>
    public class StatsHandler 
    {
        /// <summary>
        /// The sync connection.
        /// </summary>
        private readonly SyncConnection syncConnection;

        /// <summary>
        /// The storage.
        /// </summary>
        private readonly IStorage storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsHandler"/> class.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="storage">
        /// The storage.
        /// </param>
        public StatsHandler(SyncConnection connection, IStorage storage)
        {
            this.storage = storage;
            this.syncConnection = connection;
        }

        #region Public Methods and Operators

        /// <summary>
        /// The stats connection.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<StatsConnection> StatsConnection()
        {
            var connection = this.syncConnection;
            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);

            var clientConnection = await client.GetConnectionCountAsync();
            return new StatsConnection { Connections = clientConnection };
        }

        /// <summary>
        /// The stats connection.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Statistics> Statistics()
        {
            var connection = this.syncConnection;
            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);
            
            var stats = new Statistics { CoinTag = this.syncConnection.CoinTag };

            stats.ClientInfo = await client.GetInfoAsync();
            stats.TransactionsInPool = this.storage.GetMemoryTransactions().Count();
            stats.SyncBlockIndex = this.storage.BlockGetBlockCount(1).First().BlockIndex;

            return stats;
        }

        /// <summary>
        /// The stats connection.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<List<PeerInfo>> Peers()
        {
            var connection = this.syncConnection;
            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);
            return (await client.GetPeerInfo()).ToList();
        }

        #endregion
    }
}
