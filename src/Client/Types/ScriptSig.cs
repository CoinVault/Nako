// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptSig.cs" company="SoftChains">
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
    /// The script sig.
    /// </summary>
    public class ScriptSig
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the ASM.
        /// </summary>
        public string Asm { get; set; }

        /// <summary>
        /// Gets or sets the hex.
        /// </summary>
        public string Hex { get; set; }

        #endregion
    }
}