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
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    #endregion

    public class Terminator
    {
        private readonly NakoApplication nakoApplication;

        private readonly Tracer tracer;

        public Terminator(NakoApplication application, Tracer tracer)
        {
            this.tracer = tracer;
            this.nakoApplication = application;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                SetConsoleCtrlHandler(ConsoleCtrlCheck, true);

                this.tracer.Trace("Terminator", "Press any key to exist");

                this.tracer.ReadLine();

                this.tracer.Trace("Terminator", "Terminating by command");

                if (this.nakoApplication.SyncTokenSource != null)
                {
                    this.nakoApplication.SyncTokenSource.Cancel();
                }

                this.nakoApplication.ExitApplication = true;
            });
        }

        private bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            // When the application is signalled a Ctrl-C or the proccesses is terminated

            switch (ctrlType)
            {
                case CtrlTypes.CTRL_C_EVENT:
                case CtrlTypes.CTRL_BREAK_EVENT:
                case CtrlTypes.CTRL_CLOSE_EVENT:
                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:

                    this.tracer.Trace("Terminator", "Terminating by event");

                    if (this.nakoApplication.SyncTokenSource != null)
                    {
                        this.nakoApplication.SyncTokenSource.Cancel();
                    }

                    this.nakoApplication.ExitApplication = true;

                    break;
            }

            return true;
        }

        #region unmanaged

        // Declare the SetConsoleCtrlHandler function as external and receiving a delegate.
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages sent to the handler routine.
        public enum CtrlTypes
        {

            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        #endregion
    }
}
