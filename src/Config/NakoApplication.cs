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

    /// <summary>
    /// The application.
    /// </summary>
    public class NakoApplication
    {
        /// <summary>
        /// Gets or sets the cancellation token.
        /// </summary>
        public CancellationToken ApiToken { get; set; }

        /// <summary>
        /// Gets or sets the cancellation token.
        /// </summary>
        public CancellationToken SyncToken { get; set; }

        /// <summary>
        /// Gets or sets the cancellation token.
        /// </summary>
        public CancellationTokenSource ApiTokenSource { get; set; }

        /// <summary>
        /// Gets or sets the cancellation token.
        /// </summary>
        public CancellationTokenSource SyncTokenSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether exit application.
        /// </summary>
        public bool ExitApplication { get; set; }

        /// <summary>
        /// The create.
        /// </summary>
        /// <returns>
        /// The <see cref="NakoApplication"/>.
        /// </returns>
        public CancellationToken CreateApiToken()
        {
            this.ApiTokenSource = new CancellationTokenSource();

            this.ApiToken = this.ApiTokenSource.Token;

            return this.ApiToken;
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <returns>
        /// The <see cref="NakoApplication"/>.
        /// </returns>
        public CancellationToken CreateSyncToken()
        {
            this.SyncTokenSource = new CancellationTokenSource();

            this.SyncToken = this.SyncTokenSource.Token;

            return this.SyncToken;
        }
    }
}
