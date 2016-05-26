// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskRunner.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Sync.SyncTasks
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Nako.Config;

    #endregion

    public abstract class TaskRunner
    {
        private readonly NakoApplication application;

        private readonly Tracer tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRunner"/> class.
        /// </summary>
        protected TaskRunner(NakoApplication application, NakoConfiguration configuration, Tracer tracer)
        {
            this.tracer = tracer;
            this.application = application;
            this.Delay = TimeSpan.FromSeconds(configuration.BlockFinderInterval);
        }

        public TimeSpan Delay { get; set; }

        public bool Abort { get; set; }

        protected Runner Runner { get; set; }

        public Task Run(Runner runner, CancellationTokenSource tokenSource)
        {
            this.Runner = runner;
            var cancellationToken = tokenSource.Token;

            var task = Task.Run(
                async () =>
                {
                    try
                    {
                        while (!this.Abort && !this.application.ExitApplication)
                        {
                            if (await this.OnExecute())
                            {
                                cancellationToken.ThrowIfCancellationRequested();

                                continue;
                            }

                            this.tracer.Trace("TaskRunner-" + this.GetType().Name, string.Format("Delay = {0}", this.Delay.TotalSeconds), ConsoleColor.DarkRed);

                            cancellationToken.ThrowIfCancellationRequested();

                            await Task.Delay(this.Delay, cancellationToken);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // do nothing the task was cancel.
                        throw;
                    }
                    catch (Exception ex)
                    {
                        this.tracer.Trace("TaskRunner-" + this.GetType().Name, string.Format("Error = {0}", ex), ConsoleColor.Red);

                        tokenSource.Cancel();

                        throw;
                    }
                }, 
                cancellationToken);

            return task;
        }

        public abstract Task<bool> OnExecute();
    }
}