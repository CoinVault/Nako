// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncConnection.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Operations.Types
{
    #region Using Directives

    using System;

    using Nako.Config;

    #endregion

    /// <summary>
    /// The crypto connection.
    /// </summary>
    [Serializable]
    public class SyncConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncConnection"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public SyncConnection(NakoConfiguration configuration)
        {
            this.CoinTag = configuration.CoinTag;
            this.Password = configuration.RpcPassword;
            this.RpcAccessPort = configuration.RpcAccessPort;
            this.ServerDomain = configuration.RpcDomain;
            this.User = configuration.RpcUser;
            this.Secure = configuration.RpcSecure;
            this.StartBlockIndex = configuration.StartBlockIndex;
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the coin tag.
        /// </summary>
        public string CoinTag { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the AccessPort.
        /// </summary>
        public int RpcAccessPort { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connection is secure.
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        /// Gets or sets the coin tag.
        /// </summary>
        public string ServerDomain { get; set; }

        /// <summary>
        /// Gets or sets the server connection address.
        /// </summary>
        public string ServerIp { get; set; }

        /// <summary>
        /// Gets or sets the ServerName.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the RUser.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the start block index.
        /// </summary>
        public long StartBlockIndex { get; set; }

        #endregion
    }
}
