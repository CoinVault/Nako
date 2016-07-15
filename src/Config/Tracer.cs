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
    using System.Threading;

    #endregion

    public class Tracer
    {
        private readonly NakoConfiguration configuration;
        private readonly Throttler throttler;

        public Tracer(NakoConfiguration configuration)
        {
            this.configuration = configuration;
            this.throttler = new Throttler();
        }

        public void Trace(string command, string message, ConsoleColor consoleColor = ConsoleColor.Gray)
        {
            if (this.throttler.CanWrite())
            {
                //Console.ForegroundColor = consoleColor;
                Console.WriteLine("{0} {1} {2} : {3}", DateTime.UtcNow.ToString(@"d\.hh\:mm\:ss"), this.configuration.CoinTag, command, message);
            }
        }

        public void TraceError(string command, string message, ConsoleColor consoleColor = ConsoleColor.Red)
        {
            //Console.ForegroundColor = consoleColor;
            Console.WriteLine("{0} {1} {2} : {3}", DateTime.UtcNow.ToString(@"d\.hh\:mm\:ss"), this.configuration.CoinTag, command, message);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void DetailedTrace(string command, string message, ConsoleColor consoleColor = ConsoleColor.Gray)
        {
            if (this.configuration.DetailedTrace > 0)
            {
                if (this.throttler.CanWrite())
                {
                    //Console.ForegroundColor = consoleColor;
                    Console.WriteLine("{0} {1} {2} : {3}", DateTime.UtcNow.ToString(@"d\.hh\:mm\:ss"), this.configuration.CoinTag, command, message);
                }
            }
        }

        /// <summary>
        /// A throttle to skip traces when a threshold is reached.
        /// It seems when tracing too much in mono te application may hang.
        /// </summary>
        private class Throttler
        {
            private int hits;
            private int skiped;

            private DateTime minute = DateTime.Now;

            public bool CanWrite()
            {
                if (hits < 10)
                {
                    Interlocked.Increment(ref hits);
                    return true;
                }
                else
                {
                    if (DateTime.Now > minute.AddSeconds(1))
                    {
                        if (skiped > 0)
                        {
                            // a temporary trace to get stats on how many traces where skipped
                            Console.WriteLine("=========== hits {0} ========== skpied {1} ======================================", hits, skiped);
                        }
                        minute = DateTime.Now;
                        hits = 1;
                        skiped = 0;
                        return true;
                    }
                    else
                    {
                        Interlocked.Increment(ref skiped);
                        return false;
                    }
                }
            }
        }
    }
}
