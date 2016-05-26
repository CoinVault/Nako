// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tracer.cs" company="SoftChains">
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

    #endregion

    /// <summary>
    /// The tracer.
    /// </summary>
    public class Tracer
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly NakoConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public Tracer(NakoConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// The track.
        /// </summary>
        /// <param name="command">
        /// The command name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="consoleColor">
        /// The console Color.
        /// </param>
        public void Trace(string command, string message, ConsoleColor consoleColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine("{0} {1} {2} : {3}", DateTime.UtcNow.ToString(@"d\.hh\:mm\:ss"), this.configuration.CoinTag, command, message);
        }

        /// <summary>
        /// The track.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// The track.
        /// </summary>
        /// <param name="command">
        /// The command name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="consoleColor">
        /// The console Color.
        /// </param>
        public void DetailedTrace(string command, string message, ConsoleColor consoleColor = ConsoleColor.Gray)
        {
            if (this.configuration.DetailedTrace > 0)
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine("{0} {1} {2} : {3}", DateTime.UtcNow.ToString(@"d\.hh\:mm\:ss"), this.configuration.CoinTag, command, message);
            }
        }
    }
}
