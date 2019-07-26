// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandHandler.cs" company="SoftChains">
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
    public class CommandHandler
    {
        private readonly SyncConnection syncConnection;

        private readonly IStorage storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsHandler"/> class.
        /// </summary>
        public CommandHandler(SyncConnection connection, IStorage storage)
        {
            this.storage = storage;
            this.syncConnection = connection;
        }

        public async Task<string> SendTransaction(string transactionHex)
        {
            // todo: consider adding support for retries.
            // todo: check how a failure is porpageted

            var connection = this.syncConnection;
            var client = CryptoClientFactory.Create(connection.ServerDomain, connection.RpcAccessPort, connection.User, connection.Password, connection.Secure);
            var trxid = await client.SentRawTransactionAsync(transactionHex);
            return trxid;
        }
    }
}
