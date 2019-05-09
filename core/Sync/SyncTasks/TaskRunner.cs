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
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Nako.Config;

    public abstract class TaskRunner
    {
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRunner"/> class.
        /// </summary>
        protected TaskRunner(IOptions<NakoConfiguration> configuration, ILogger logger)
        {
            this.log = logger;
            this.Delay = TimeSpan.FromSeconds(configuration.Value.SyncInterval);
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
                        while (!this.Abort)
                        {
                            if (await this.OnExecute())
                            {
                                cancellationToken.ThrowIfCancellationRequested();

                                continue;
                            }

                            this.log.LogDebug($"TaskRunner-{GetType().Name} Delay = {Delay.TotalSeconds}");

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
                        this.log.LogError(ex, $"TaskRunner-{GetType().Name}");

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