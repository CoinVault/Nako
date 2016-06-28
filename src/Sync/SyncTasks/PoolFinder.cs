// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PoolFinder.cs" company="SoftChains">
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
    using System.Linq;
    using System.Threading.Tasks;

    using Nako.Config;
    using Nako.Extensions;
    using Nako.Operations;
    using Nako.Operations.Types;

    #endregion

    public class PoolFinder : TaskRunner
    {
        private readonly NakoConfiguration config;

        private readonly ISyncOperations syncOperations;

        private readonly SyncConnection syncConnection;

        private readonly Tracer tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PoolFinder"/> class.
        /// </summary>
        public PoolFinder(NakoApplication application, NakoConfiguration config, ISyncOperations syncOperations, SyncConnection syncConnection, Tracer tracer)
            : base(application, config, tracer)
        {
            this.tracer = tracer;
            this.syncConnection = syncConnection;
            this.syncOperations = syncOperations;
            this.config = config;
        }

        /// <inheritdoc />
        public override async Task<bool> OnExecute()
        {
            if (!this.config.SyncMemoryPool)
            {
                this.Abort = true;
                return true;
            }

            var syncingBlocks = this.Runner.SyncingBlocks;

            if (syncingBlocks.Blocked)
            {
                return false;
            }

            if (this.Runner.Get<BlockSyncer>().Queue.Count() >= this.config.MaxItemsInQueue)
            {
                return false;
            }

            var stoper = Stopwatch.Start();

            var pool = this.syncOperations.FindPoolTransactions(this.syncConnection, syncingBlocks).Result;

            if (!pool.Transactions.Any())
            {
                return false;
            }

            stoper.Stop();

            this.tracer.Trace("PoolFinder", string.Format("Seconds = {0} - New Transactions = {1}/{2}", stoper.Elapsed.TotalSeconds, pool.Transactions.Count, syncingBlocks.CurrentPoolSyncing.Count()), ConsoleColor.DarkYellow);

            this.Runner.Get<BlockSyncer>().Enqueue(new SyncBlockOperation { PoolTransactions = pool });

            return await Task.FromResult(false);
        }
    }
}
