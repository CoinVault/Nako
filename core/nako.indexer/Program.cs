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
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Nako.Config;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// The application program.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builder, config) =>
                {
                    config.AddJsonFile("nakosettings.json", optional: false, reloadOnChange: true);

                    // If the argument is only one, and it does not contain = or -, then we'll for backwards compatibility, use that as CoinTag.
                    if (args.Length == 1 && !args[0].Contains("=") && !args[0].Contains("-"))
                    {
                        config.AddInMemoryCollection(new Dictionary<string, string> { { "CoinTag", args[0].ToUpper() } });
                    }
                })
               .UseStartup<Startup>()
               .Build();
    }
}