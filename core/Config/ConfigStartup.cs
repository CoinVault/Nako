// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigStartup.cs" company="SoftChains">
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
    // using System.Configuration;
    using System.IO;
    using System.Linq;

    using Nako.Storage;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using Microsoft.Extensions.PlatformAbstractions;

    #endregion

    public class ConfigStartup
    {
        /// <summary>
        /// The load configuration.
        /// </summary>
        public static NakoConfiguration LoadConfiguration(string[] args)
        {
            var path = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "nakosettings.json");
            if (!File.Exists(path))
            {
                throw new Exception("Path not found");
            }
            var rawConfig = File.ReadAllText(path);
            var nakoConfig = JsonConvert.DeserializeObject<NakoConfiguration>(rawConfig);
            
            var coinTag = nakoConfig.CoinTag;
            
            if (args.Any())
            {
                coinTag = args.First();
            }

            if (string.IsNullOrEmpty(coinTag) || coinTag.StartsWith("=="))
            {
                throw new Exception("Please provide a CoinTag in config or as argument");
            }

            coinTag = coinTag.ToUpper();
            nakoConfig.CoinTag = coinTag;
            nakoConfig.ConnectionString = nakoConfig.ConnectionString.Replace("{CoinTag}", coinTag.ToLower());
            nakoConfig.RpcDomain = nakoConfig.RpcDomain.Replace("{CoinTag}", coinTag.ToLower());
            
            return nakoConfig;
        }

        private static string GetConfigPath(string[] args)
        {
            var location = string.Empty;

            if (args.Any())
            {
                location = args.First();

                if (!File.Exists(location))
                {
                    Console.WriteLine("Config file not found {0}", location);
                    Console.ReadKey();
                    throw new Exception("Path not found");
                }
            }

            return location;
        }
    }
}
