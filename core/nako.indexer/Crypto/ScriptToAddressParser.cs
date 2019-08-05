// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorage.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using NBitcoin;

namespace Nako.Crypto
{
    public class ScriptToAddressParser
    {
        public static string[] GetAddress(Network network, Script script)
        {
            var template = NBitcoin.StandardScripts.GetTemplateFromScriptPubKey(script);

            if (template == null)
                return null;

            if (template.Type == TxOutType.TX_NONSTANDARD)
                return null;

            if (template.Type == TxOutType.TX_NULL_DATA)
                return null;

            if (template.Type == TxOutType.TX_PUBKEY)
            {
                var pubkeys = script.GetDestinationPublicKeys(network);
                return new[] {pubkeys[0].GetAddress(network).ToString()};
            }

            if (template.Type == TxOutType.TX_PUBKEYHASH ||
                template.Type == TxOutType.TX_SCRIPTHASH ||
                template.Type == TxOutType.TX_SEGWIT)
            {
                BitcoinAddress bitcoinAddress = script.GetDestinationAddress(network);
                if (bitcoinAddress != null)
                {
                    return new[] { bitcoinAddress.ToString() };
                }
            }

            if (template.Type == TxOutType.TX_MULTISIG)
            {
                // TODO;
                return null;
            }

            if (template.Type == TxOutType.TX_COLDSTAKE)
            {
                if (ColdStakingScriptTemplate.Instance.ExtractScriptPubKeyParameters(script, out KeyId hotPubKeyHash, out KeyId coldPubKeyHash))
                {
                    // We want to index based on both the cold and hot key
                    return new[]
                    {
                        hotPubKeyHash.GetAddress(network).ToString(),
                        coldPubKeyHash.GetAddress(network).ToString(),
                    };
                }

                return null;
            }

            return null;
        }
    }
}

