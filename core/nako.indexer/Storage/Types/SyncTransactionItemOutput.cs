// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncTransactionItemOutput.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Nako.Storage.Types
{
    public class SyncTransactionItemOutput
    {
        #region Public Properties

        public int Index { get; set; }

        public string Address { get; set; }

        public string OutputType { get; set; }

        public long Value { get; set; }

        public string ScriptPubKey { get; set; }

        #endregion
    }
}