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

    public abstract class TaskStarter
    {
        private readonly Tracer tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskStarter"/> class.
        /// </summary>
        protected TaskStarter(Tracer tracer)
        {
            this.tracer = tracer;
        }

        public abstract int Priority { get; }

        protected Runner Runner { get; set; }

        /// <summary>
        /// The run.
        /// </summary>
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

        public abstract Task OnExecute();
    }
}