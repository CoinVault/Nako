// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskStarter.cs" company="SoftChains">
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
    public abstract class TaskStarter
    {
        /// <summary>
        /// The tracer.
        /// </summary>
        private readonly Tracer tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskStarter"/> class.
        /// </summary>
        /// <param name="tracer">
        /// The tracer.
        /// </param>
        protected TaskStarter(Tracer tracer)
        {
            this.tracer = tracer;
        }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        public abstract int Priority { get; }

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
                        await this.OnExecute();

                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException)
                    {
                        // do nothing the task was cancel.
                        throw;
                    }
                    catch (Exception ex)
                    {
                        this.tracer.Trace("TaskStarter-" + this.GetType().Name, string.Format("Error = {0}", ex), ConsoleColor.Red);

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
        public abstract Task OnExecute();
    }
}