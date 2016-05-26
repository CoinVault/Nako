// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignRawTransactionInput.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Client.Types
{
    #region Using Directives

    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// The sign raw transaction input.
    /// </summary>
    public class SignRawTransactionInput
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        [JsonProperty("vout")]
        public int Output { get; set; }

        /// <summary>
        /// Gets or sets the script pub key.
        /// </summary>
        [JsonProperty("scriptPubKey")]
        public string ScriptPubKey { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        #endregion
    }
}
