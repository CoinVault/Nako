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
    using System;
    using Microsoft.Extensions.Options;
    using Nako.Config;


    [Serializable]
    public class SyncConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncConnection"/> class.
        /// </summary>
        public SyncConnection(IOptions<NakoConfiguration> config)
        {
            NakoConfiguration configuration = config.Value;

            this.CoinTag = configuration.CoinTag;
            this.Password = configuration.RpcPassword;
            this.RpcAccessPort = configuration.RpcAccessPort;
            this.ServerDomain = configuration.RpcDomain;
            this.User = configuration.RpcUser;
            this.Secure = configuration.RpcSecure;
            this.StartBlockIndex = configuration.StartBlockIndex;
        }


        public string CoinTag { get; set; }

        public string Password { get; set; }

        public int RpcAccessPort { get; set; }

        public bool Secure { get; set; }

        public string ServerDomain { get; set; }

        public string ServerIp { get; set; }

        public string ServerName { get; set; }

        public string User { get; set; }

        public long StartBlockIndex { get; set; }

    }
}
