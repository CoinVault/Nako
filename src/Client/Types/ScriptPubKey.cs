// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptPubKey.cs" company="SoftChains">
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
    /// The script pub key.
    /// </summary>
    public class ScriptPubKey
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        public List<string> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the ASM.
        /// </summary>
        public string Asm { get; set; }

        /// <summary>
        /// Gets or sets the hex.
        /// </summary>
        public string Hex { get; set; }

        /// <summary>
        /// Gets or sets the registration signature.
        /// </summary>
        public int ReqSigs { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type { get; set; }

        #endregion
    }
}