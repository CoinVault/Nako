// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NakoConfiguration.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using Newtonsoft.Json;

namespace Nako.Config
{
    public class NakoConfiguration
    {
        public string CoinTag { get; set; }

        public string RpcPassword { get; set; }

        public int SyncInterval { get; set; }

        public int MaxItemsInQueue { get; set; }

        public int ParallelRequestsToTransactionRpc { get; set; }

        public int RpcAccessPort { get; set; }

        public bool RpcSecure { get; set; }

        public string RpcDomain { get; set; }

        public bool SyncBlockchain { get; set; }

        public long StartBlockIndex { get; set; }

        public bool SyncMemoryPool { get; set; }

        public string RpcUser { get; set; }

        public string NotifyUrl { get; set; }

        public string ConnectionString { get; set; }
        
        public int NotifyBatchCount { get; set; }

        public int MongoBatchSize { get; set; }

        public int AverageInterval { get; set; }

        public string NetworkConsensusFactoryType { get; set; }

        public int NetworkPubkeyAddressPrefix { get; set; }

        public int NetworkScriptAddressPrefix { get; set; }

        public string NetworkWitnessPrefix { get; set; }

        [JsonIgnore]
        public string ConnectionStringActual { get { return ConnectionString.Replace("{CoinTag}", CoinTag.ToLower()); } }

        [JsonIgnore]
        public string RpcDomainActual { get { return RpcDomain.Replace("{CoinTag}", CoinTag.ToLower()); } }
    }
}
