// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncTransactionItemInput.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Nako.Storage.Types
{
    public class SyncTransactionItemInput
    {
        #region Public Properties

        public string InputCoinBase { get; set; }

        public int PreviousIndex { get; set; }

        public string PreviousTransactionHash { get; set; }

        public string ScriptSig { get; set; }

        public string WitScript { get; set; }

        public string SequenceLock { get; set; }

        #endregion
    }
}