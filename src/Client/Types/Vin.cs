// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vin.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Nako.Client.Types
{
    public class Vin
    {
        #region Public Properties

        public ScriptSig ScriptSig { get; set; }

        public long Sequence { get; set; }

        public string CoinBase { get; set; }

        public string TxId { get; set; }

        public int VOut { get; set; }

        #endregion
    }
}