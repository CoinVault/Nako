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
    using System;
    using System.IO;

    /// <summary>
    /// The application program.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var chain = (args.Length == 0) ? "STRAT" : args[0].ToUpper();
            chain = (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("CHAIN"))) ? chain : Environment.GetEnvironmentVariable("CHAIN").ToUpper();

            var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("hosting.json", optional: true)
              .AddJsonFile("nakosettings.json", optional: false, reloadOnChange: false)
              .AddJsonFile(Path.Combine("Setup", $"{chain}.json"), optional: true, reloadOnChange: false)
              .AddCommandLine(args)
              .AddEnvironmentVariables()
              .Build();

            WebHost.CreateDefaultBuilder(args)
               .UseConfiguration(config)
               .UseStartup<Startup>()
               .Build().Run();
        }
    }
}