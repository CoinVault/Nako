// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NakoApplication.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Nako.Config
{
    #region Using Directives

    using System.Threading;

    #endregion

    public class NakoApplication
    {
        public CancellationToken ApiToken { get; set; }

        public CancellationToken SyncToken { get; set; }

        public CancellationTokenSource ApiTokenSource { get; set; }

        public CancellationTokenSource SyncTokenSource { get; set; }

        public bool ExitApplication { get; set; }

        public CancellationToken CreateApiToken()
        {
            this.ApiTokenSource = new CancellationTokenSource();

            this.ApiToken = this.ApiTokenSource.Token;

            return this.ApiToken;
        }

        public CancellationToken CreateSyncToken()
        {
            this.SyncTokenSource = new CancellationTokenSource();

            this.SyncToken = this.SyncTokenSource.Token;

            return this.SyncToken;
        }
    }
}
