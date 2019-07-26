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
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Nako.Config;


    public abstract class TaskStarter
    {
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskStarter"/> class.
        /// </summary>
        protected TaskStarter(ILogger logger)
        {
            this.log = logger;
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
                        this.log.LogError(ex, "TaskStarter-" + this.GetType().Name);

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