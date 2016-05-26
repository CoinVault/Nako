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

    using Nako.Storage;

    #endregion

    /// <summary>
    /// The config.
    /// </summary>
    public class ConfigStartup
    {
        /// <summary>
        /// The load configuration.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// The <see cref="NakoConfiguration"/>.
        /// </returns>
        public static NakoConfiguration LoadConfiguration(string location)
        {
            if (location != null)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.File = location;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }

            Common.ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

            var nakoConfig  = new NakoConfiguration
            {
                CoinTag = ConfigurationManager.AppSettings.Get("CoinTag"), 
                
                RpcPassword = ConfigurationManager.AppSettings.Get("RpcPassword"), 
                RpcAccessPort = Convert.ToInt16(ConfigurationManager.AppSettings.Get("RpcAccessPort")), 
                RpcUser = ConfigurationManager.AppSettings.Get("RpcUser"), 
                RpcDomain = ConfigurationManager.AppSettings.Get("RpcDomain"), 
                RpcSecure = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("RpcSecure")), 

                BlockFinderInterval = Convert.ToInt16(ConfigurationManager.AppSettings.Get("BlockFinderInterval")), 
                BlockStoreInterval = Convert.ToInt16(ConfigurationManager.AppSettings.Get("BlockStoreInterval")), 
                BlockSyncerInterval = Convert.ToInt16(ConfigurationManager.AppSettings.Get("BlockSyncerInterval")), 
                SyncBlockchain = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("SyncBlockchain")), 
                SyncMemoryPool = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("SyncMemoryPool")), 
                StartBlockIndex = Convert.ToInt64(ConfigurationManager.AppSettings.Get("StartBlockIndex")), 
                MaxItemsInQueue = Convert.ToInt16(ConfigurationManager.AppSettings.Get("MaxItemsInQueue")), 
                ParallelRequestsToTransactionRpc = Convert.ToInt16(ConfigurationManager.AppSettings.Get("ParallelRequestsToTransactionRpc")), 
                SyncApiPort = Convert.ToInt32(ConfigurationManager.AppSettings.Get("SyncApiPort")), 
                DetailedTrace = Convert.ToInt32(ConfigurationManager.AppSettings.Get("DetailedTrace")), 
                ParalleleTableStorageBatchCount = Convert.ToInt32(ConfigurationManager.AppSettings.Get("ParalleleTableStorageBatchCount")), 
                NotifyUrl = ConfigurationManager.AppSettings.Get("NotifyUrl"), 
                NotifyBatchCount = Convert.ToInt16(ConfigurationManager.AppSettings.Get("NotifyBatchCount"))
            };

            return nakoConfig;
        }
    }
}
