// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Terminator.cs" company="SoftChains">
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

    using System;
    using System.Threading.Tasks;

    #endregion

    /// <summary>
    /// The terminator.
    /// </summary>
    public class Terminator
    {
        /// <summary>
        /// The application.
        /// </summary>
        private readonly NakoApplication nakoApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="Terminator"/> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        public Terminator(NakoApplication application)
        {
            this.nakoApplication = application;
        }

        /// <summary>
        /// The start.
        /// </summary>
        public void Start()
        {
            Task.Run(() =>
            {
                Console.WriteLine("Press any key to exist");
                Console.Read();
                
                if (this.nakoApplication.SyncTokenSource != null)
                {
                    this.nakoApplication.SyncTokenSource.Cancel();
                }

                this.nakoApplication.ExitApplication = true;
            });
        }
    }
}
