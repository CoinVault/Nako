// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignRawTransaction.cs" company="SoftChains">
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

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The sign raw transaction.
    /// </summary>
    public class SignRawTransaction
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SignRawTransaction"/> class.
        /// </summary>
        /// <param name="rawTransactionHex">
        /// The raw transaction hex.
        /// </param>
        public SignRawTransaction(string rawTransactionHex)
        {
            this.RawTransactionHex = rawTransactionHex;
            this.Inputs = new List<SignRawTransactionInput>();
            this.PrivateKeys = new List<string>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets A list of explicitly specified inputs to sign. This can be used
        /// if you do not want to sign all inputs in this transaction just yet.
        /// </summary>
        public List<SignRawTransactionInput> Inputs { get; set; }

        /// <summary>
        /// Gets or sets A list with the private keys needed to sign the transaction.
        /// There keys only have to be included if they are not in the wallet.
        /// </summary>
        public List<string> PrivateKeys { get; set; }

        /// <summary>
        /// Gets or sets the hexadecimal encoded version of the raw transaction to sign.
        /// </summary>
        public string RawTransactionHex { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add input.
        /// </summary>
        /// <param name="transactionId">
        /// The transaction id.
        /// </param>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <param name="scriptPubKey">
        /// The script pub key.
        /// </param>
        public void AddInput(string transactionId, int output, string scriptPubKey)
        {
            this.Inputs.Add(new SignRawTransactionInput { TransactionId = transactionId, Output = output, ScriptPubKey = scriptPubKey });
        }

        /// <summary>
        /// The add key.
        /// </summary>
        /// <param name="privateKey">
        /// The private key.
        /// </param>
        public void AddKey(string privateKey)
        {
            this.PrivateKeys.Add(privateKey);
        }

        #endregion
    }
}
