// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryTransactionOutput.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Api.Handlers.Types
{
    public class QueryTransactionOutput
    {
        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public long Balance { get; set; }

        /// <summary>
        /// Gets or sets the input index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the output type.
        /// </summary>
        public string OutputType { get; set; }

        public string ScriptPubKeyAsm { get; set; }

        public string ScriptPubKey { get; set; }

        public string SpentInTransaction { get; set; }
    }
}
