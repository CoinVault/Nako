// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncConnection.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using NBitcoin;
using NBitcoin.DataEncoders;

namespace Nako.Operations.Types
{
    using System;
    using Microsoft.Extensions.Options;
    using Nako.Config;

    /// <summary>
    /// Represents a minimal 
    /// </summary>
    public class NetworkConfig : Network
    {
        public NetworkConfig(NakoConfiguration config)
        {
            this.CoinTicker = config.CoinTag;

            ConsensusFactory consensusFactory = (ConsensusFactory)Activator.CreateInstance(Type.GetType(config.NetworkConsensusFactoryType));
                
            this.Consensus = new ConsensusConfig(config, consensusFactory);

            this.Base58Prefixes = new byte[12][];
            this.Base58Prefixes[(int)Base58Type.PUBKEY_ADDRESS] = new byte[] { (config.NetworkPubkeyAddressPrefix) };
            this.Base58Prefixes[(int)Base58Type.SCRIPT_ADDRESS] = new byte[] { (config.NetworkScriptAddressPrefix) };

            this.Bech32Encoders = new Bech32Encoder[2];
            var encoder = new Bech32Encoder(config.NetworkWitnessPrefix);
            this.Bech32Encoders[(int)Bech32Type.WITNESS_PUBKEY_ADDRESS] = encoder;
            this.Bech32Encoders[(int)Bech32Type.WITNESS_SCRIPT_ADDRESS] = encoder;

            // TODO
            //StandardScripts.RegisterStandardScriptTemplate(ColdStakingScriptTemplate);
        }
    }

    public class ConsensusConfig : Consensus
    {
        public ConsensusConfig(NakoConfiguration config, ConsensusFactory consensusFactory) : base(
            consensusFactory: consensusFactory,
            consensusOptions: null,
            coinType: 0,
            hashGenesisBlock: uint256.Zero,
            subsidyHalvingInterval: 0,
            majorityEnforceBlockUpgrade: 0,
            majorityRejectBlockOutdated: 0,
            majorityWindow: 0,
            buriedDeployments: null,
            bip9Deployments: null,
            bip34Hash: uint256.Zero,
            ruleChangeActivationThreshold: 0,
            minerConfirmationWindow: 0,
            maxReorgLength: 0,
            defaultAssumeValid: uint256.Zero,
            maxMoney: 0,
            coinbaseMaturity: 0,
            premineHeight: 0,
            premineReward: 0,
            proofOfWorkReward: 0,
            powTargetTimespan: TimeSpan.Zero,
            powTargetSpacing: TimeSpan.Zero,
            powAllowMinDifficultyBlocks: false,
            posNoRetargeting: false,
            powNoRetargeting: false,
            powLimit: new Target(uint256.Zero),
            minimumChainWork: null,
            isProofOfStake: consensusFactory is PosConsensusFactory,
            lastPowBlock: 0,
            proofOfStakeLimit: null,
            proofOfStakeLimitV2: null,
            proofOfStakeReward: 0
        )
        {
        }
    }

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
            this.ServerDomain = configuration.RpcDomainActual;
            this.User = configuration.RpcUser;
            this.Secure = configuration.RpcSecure;
            this.StartBlockIndex = configuration.StartBlockIndex;

            // This can be replaced with a specific the network class of a specific coin
            // Or use the config values to simulate the network class.
            this.Network = new NetworkConfig(config.Value);

            this.RecentItems = new Buffer<(DateTime Inserted, TimeSpan Duration, long Size)>(5000);
        }

        public NBitcoin.Network Network { get; }

        public string CoinTag { get; set; }

        public string Password { get; set; }

        public int RpcAccessPort { get; set; }

        public bool Secure { get; set; }

        public string ServerDomain { get; set; }

        public string ServerIp { get; set; }

        public string ServerName { get; set; }

        public string User { get; set; }

        public long StartBlockIndex { get; set; }

        public Buffer<(DateTime Inserted, TimeSpan Duration, long Size)> RecentItems { get; set; }
    }

    public class Buffer<T> : Queue<T>
    {
        private int? maxCapacity { get; set; }

        public Buffer() { maxCapacity = null; }
        public Buffer(int capacity) { maxCapacity = capacity; }

        public void Add(T newElement)
        {
            if (this.Count == (maxCapacity ?? -1)) this.Dequeue(); // no limit if maxCapacity = null
            this.Enqueue(newElement);
        }
    }
}
