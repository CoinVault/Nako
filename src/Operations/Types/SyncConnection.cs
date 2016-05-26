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

    [Serializable]
    public class SyncConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncConnection"/> class.
        /// </summary>
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

        public string CoinTag { get; set; }

        public string Password { get; set; }

        public int RpcAccessPort { get; set; }

        public bool Secure { get; set; }

        public string ServerDomain { get; set; }

        public string ServerIp { get; set; }

        public string ServerName { get; set; }

        public string User { get; set; }

        public long StartBlockIndex { get; set; }

        #endregion
    }
}
