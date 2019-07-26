// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncServer.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Sync
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Nako.Config;
    using Nako.Sync.SyncTasks;

    /// <summary>
    /// The processes responsible of triggering sync tasks.
    /// </summary>
    public class SyncServer : IHostedService, IDisposable
    {
        private readonly NakoConfiguration configuration;
        private readonly ILogger<SyncServer> log;
        private readonly IServiceScopeFactory scopeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncServer"/> class.
        /// </summary>
        public SyncServer(ILogger<SyncServer> logger, IOptions<NakoConfiguration> configuration, IServiceScopeFactory scopeFactory)
        {
            this.log = logger;
            this.configuration = configuration.Value;
            this.scopeFactory = scopeFactory;
        }

        public void Dispose()
        {
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.log.LogInformation($"Start sync for {configuration.CoinTag}");
            log.LogInformation("Starting the Sync Service...");

            Task.Run(async () =>
            {
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var tokenSource = new CancellationTokenSource();
                        cancellationToken.Register(() => { tokenSource.Cancel(); });

                        try
                        {
                            using (var scope = scopeFactory.CreateScope())
                            {
                                var runner = scope.ServiceProvider.GetService<Runner>();
                                var runningTasks = runner.RunAll(tokenSource);

                                Task.WaitAll(runningTasks.ToArray(), cancellationToken);

                                if (cancellationToken.IsCancellationRequested)
                                {
                                    tokenSource.Cancel();
                                }
                            }

                            break;
                        }
                        catch (OperationCanceledException)
                        {
                            // do nothing the task was cancel.
                            throw;
                        }
                        catch (AggregateException ae)
                        {
                            if (ae.Flatten().InnerExceptions.OfType<SyncRestartException>().Any())
                            {
                                this.log.LogInformation("Sync: ### - Restart requested - ###");
                                this.log.LogTrace("Sync: Signalling token cancelation");
                                tokenSource.Cancel();

                                continue;
                            }

                            foreach (var innerException in ae.Flatten().InnerExceptions)
                            {
                                this.log.LogError(innerException, "Sync");
                            }

                            tokenSource.Cancel();

                            var retryInterval = 10;

                            this.log.LogWarning($"Unexpected error retry in {retryInterval} seconds");
                            //this.tracer.ReadLine();

                            // Nako is designed to be idempotent, we want to continue running even if errors are found.
                            // so if an unepxected error happened we log it wait and start again

                            Task.Delay(TimeSpan.FromSeconds(retryInterval), cancellationToken).Wait(cancellationToken);

                            continue;
                        }
                        catch (Exception ex)
                        {
                            this.log.LogError(ex, "Sync");
                            break;
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // do nothing the task was cancel.
                    throw;
                }
                catch (Exception ex)
                {
                    this.log.LogError(ex, "Sync");
                    throw;
                }

            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
