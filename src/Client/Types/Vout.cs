// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vout.cs" company="SoftChains">
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
    /// The out.
    /// </summary>
    public class Vout
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the n.
        /// </summary>
        public int N { get; set; }

        /// <summary>
        /// Gets or sets the script pub key.
        /// </summary>
        public ScriptPubKey ScriptPubKey { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public decimal Value { get; set; }

        #endregion
    }
}