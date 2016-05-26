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

    public class Terminator
    {
        private readonly NakoApplication nakoApplication;

        public Terminator(NakoApplication application)
        {
            this.nakoApplication = application;
        }

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
