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

    /// <summary>
    /// The task runner.
    /// </summary>
    public abstract class TaskRunner
    {
        /// <summary>
        /// The config.
        /// </summary>
        private readonly NakoApplication application;

        /// <summary>
        /// The tracer.
        /// </summary>
        private readonly Tracer tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRunner"/> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="tracer">
        /// The tracer.
        /// </param>
        protected TaskRunner(NakoApplication application, NakoConfiguration configuration, Tracer tracer)
        {
            this.tracer = tracer;
            this.application = application;
            this.Delay = TimeSpan.FromSeconds(configuration.BlockFinderInterval);
        }

        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        public TimeSpan Delay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether abort.
        /// </summary>
        public bool Abort { get; set; }

        /// <summary>
        /// Gets or sets the runner.
        /// </summary>
        protected Runner Runner { get; set; }

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="runner">
        /// The runner.
        /// </param>
        /// <param name="tokenSource">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
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

        /// <summary>
        /// The on execute.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public abstract Task<bool> OnExecute();
    }
}