// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateRawTransactionOutput.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Client.Types
{
    /// <summary>
    /// The create raw transaction input.
    /// </summary>
    public class CreateRawTransactionOutput 
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        public decimal Amount { get; set; }

        #endregion
    }
}
