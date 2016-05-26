
namespace Nako.Config
{
    #region Using Directives

    using System;

    #endregion

    public class Tracer
    {
        private readonly NakoConfiguration configuration;

        public Tracer(NakoConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Trace(string command, string message, ConsoleColor consoleColor = ConsoleColor.Gray)
        {
            Console.ForegroundColor = consoleColor;
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
                Console.ForegroundColor = consoleColor;
                Console.WriteLine("{0} {1} {2} : {3}", DateTime.UtcNow.ToString(@"d\.hh\:mm\:ss"), this.configuration.CoinTag, command, message);
            }
        }
    }
}
