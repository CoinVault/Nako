// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Autofac;

    using Nako.Api;
    using Nako.Config;
    using Nako.Sync;

    #endregion

    /// <summary>
    /// The application program.
    /// </summary>
    internal class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point.
        /// </summary>
        internal static void Main(string[] args)
        {
            if (!IsRunningOnMono())
            {
                Console.BufferHeight = 1000;
                Console.WindowHeight = 25;
                Console.WindowWidth = 150;
            }

            var builder = new ContainerBuilder();
            builder.RegisterInstance(ConfigStartup.LoadConfiguration(args)).As<NakoConfiguration>();
            builder.RegisterAssemblyModules(Assembly.GetEntryAssembly());
            var container = builder.Build();

            container.Resolve<ApiServer>().StartApi(container);
            container.Resolve<SyncServer>().StartSync(container);
            container.Resolve<Terminator>().Start();

            container.Resolve<NakoApplication>().SyncToken.WaitHandle.WaitOne();
            container.Resolve<NakoApplication>().ApiTokenSource.Cancel();
            container.Resolve<NakoApplication>().ApiToken.WaitHandle.WaitOne();

            //Console.Read();
        }

        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        #endregion
    }
}