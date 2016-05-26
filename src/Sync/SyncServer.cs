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
    #region Using Directives

    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Autofac;

    using Nako.Config;
    using Nako.Sync.SyncTasks;

    #endregion

    /// <summary>
    /// The processes responsible of triggering sync tasks.
    /// </summary>
    public class SyncServer
    {
        #region Fields

        private readonly NakoConfiguration configuration;

        private readonly NakoApplication application;

        private readonly Tracer tracer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncServer"/> class.
        /// </summary>
        public SyncServer(NakoConfiguration nakoConfiguration, NakoApplication nakoApplication, Tracer tracer)
        {
            this.tracer = tracer;
            this.application = nakoApplication;
            this.configuration = nakoConfiguration;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Start the sync server.
        /// </summary>
        public Task StartSync(IContainer container)
        {
            this.tracer.Trace("Sync", string.Format("Start sync for {0} ", this.configuration.CoinTag));

            return Task.Run(
                () =>
                {
                    try
                    {
                        while (!this.application.ExitApplication && !this.application.SyncToken.IsCancellationRequested)
                        {
                            var tokenSource = new CancellationTokenSource();
                            this.application.SyncToken.Register(() => { tokenSource.Cancel(); });

                            try
                            {
                                using (var scope = container.BeginLifetimeScope())
                                {
                                    var runner = scope.Resolve<Runner>();
                                    var runningTasks = runner.RunAll(tokenSource);

                                    Task.WaitAll(runningTasks.ToArray(), this.application.SyncToken);

                                    if (this.application.SyncToken.IsCancellationRequested)
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
                                    this.tracer.Trace("Sync", "### - Restart requested - ###");

                                    this.tracer.Trace("Sync", "Signalling token cancelation");
                                    tokenSource.Cancel();

                                    continue;
                                }

                                foreach (var innerException in ae.Flatten().InnerExceptions)
                                {
                                    this.tracer.Trace("Sync", innerException.ToString());
                                }

                                this.tracer.Trace("Sync", "Signalling token cancelation");

                                tokenSource.Cancel();

                                this.tracer.Trace("Sync", "Press any key to exist");
                                this.tracer.ReadLine();

                                break;
                            }
                            catch (Exception ex)
                            {
                                this.tracer.Trace("Sync", ex.ToString());
                                this.tracer.Trace("Sync", "Press any key to exist");
                                this.tracer.ReadLine();
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
                        this.tracer.Trace("Sync", ex.ToString());
                        this.tracer.ReadLine();
                        throw;
                    }
                }, 
                this.application.CreateSyncToken());
        }

        #endregion
    }
}
