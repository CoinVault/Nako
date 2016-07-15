// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigStartup.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Config
{
    #region Using Directives

    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;

    using Nako.Storage;

    #endregion

    public class ConfigStartup
    {
        /// <summary>
        /// The load configuration.
        /// </summary>
        public static NakoConfiguration LoadConfiguration(string[] args)
        {
            ////var location = GetConfigPath(args);

            ////if (!string.IsNullOrEmpty(location))
            ////{
            ////    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ////    config.AppSettings.File = location;
            ////    config.Save(ConfigurationSaveMode.Modified);
            ////    ConfigurationManager.RefreshSection("appSettings");
            ////}


            var coinTag = ConfigurationManager.AppSettings.Get("CoinTag");
            
            if (args.Any())
            {
                coinTag = args.First();
            }

            if (string.IsNullOrEmpty(coinTag) || coinTag.StartsWith("=="))
            {
                throw new ApplicationException("Please provide a CoinTag in config or as argument");
            }

            coinTag = coinTag.ToUpper();

            var nakoConfig = new NakoConfiguration
            {
                CoinTag = coinTag,

                ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString").Replace("{CoinTag}", coinTag.ToLower()),

                RpcDomain = ConfigurationManager.AppSettings.Get("RpcDomain").Replace("{CoinTag}", coinTag.ToLower()),
                RpcPassword = ConfigurationManager.AppSettings.Get("RpcPassword"),
                RpcAccessPort = Convert.ToInt16(ConfigurationManager.AppSettings.Get("RpcAccessPort")),
                RpcUser = ConfigurationManager.AppSettings.Get("RpcUser"),
                RpcSecure = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("RpcSecure")),
                
                SyncApiPort = Convert.ToInt32(ConfigurationManager.AppSettings.Get("SyncApiPort")),
                SyncBlockchain = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("SyncBlockchain")),
                SyncMemoryPool = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("SyncMemoryPool")),
                StartBlockIndex = Convert.ToInt64(ConfigurationManager.AppSettings.Get("StartBlockIndex")),
                
                NotifyUrl = ConfigurationManager.AppSettings.Get("NotifyUrl"),
                NotifyBatchCount = Convert.ToInt16(ConfigurationManager.AppSettings.Get("NotifyBatchCount")),

                DetailedTrace = Convert.ToInt32(ConfigurationManager.AppSettings.Get("DetailedTrace")),
                SyncInterval = Convert.ToInt16(ConfigurationManager.AppSettings.Get("SyncInterval")),
                MaxItemsInQueue = Convert.ToInt16(ConfigurationManager.AppSettings.Get("MaxItemsInQueue")),
                MongoBatchSize = Convert.ToInt16(ConfigurationManager.AppSettings.Get("MongoBatchSize")),
                ParallelRequestsToTransactionRpc = Convert.ToInt16(ConfigurationManager.AppSettings.Get("ParallelRequestsToTransactionRpc")),
            };

            return nakoConfig;
        }

        private static string GetConfigPath(string[] args)
        {
            var location = string.Empty;

            if (args.Any())
            {
                location = args.First();

                if (!File.Exists(location))
                {
                    Console.WriteLine("Config file not found {0}", location);
                    Console.ReadKey();
                    throw new ApplicationException("Path not found");
                }
            }

            return location;
        }
    }
}
