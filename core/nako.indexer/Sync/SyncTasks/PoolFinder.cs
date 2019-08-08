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
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Nako.Config;
    using Nako.Extensions;
    using Nako.Operations;
    using Nako.Operations.Types;

    public class PoolFinder : TaskRunner
    {
        private readonly NakoConfiguration config;

        private readonly ISyncOperations syncOperations;

        private readonly SyncConnection syncConnection;

        private readonly ILogger<PoolFinder> log;

        /// <summary>
        /// Initializes a new instance of the <see cref="PoolFinder"/> class.
        /// </summary>
        public PoolFinder(IOptions<NakoConfiguration> configuration, ISyncOperations syncOperations, SyncConnection syncConnection, ILogger<PoolFinder> logger)
            : base(configuration, logger)
        {
            this.log = logger;
            this.syncConnection = syncConnection;
            this.syncOperations = syncOperations;
            this.config = configuration.Value;
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

            if (syncingBlocks.LastBlock == null || syncingBlocks.LastBlock.Height + 10 < syncingBlocks.LastClientBlockIndex)
            {
                // Don't sync mempool until api is at tip
                return false;
            }

            if (this.Runner.Get<BlockSyncer>().Queue.Count() >= this.config.MaxItemsInQueue)
            {
                return false;
            }

            var stoper = Stopwatch.Start();

            var pool = this.syncOperations.FindPoolTransactions(this.syncConnection, syncingBlocks);

            if (!pool.Transactions.Any())
            {
                return false;
            }

            stoper.Stop();

            this.log.LogDebug($"Seconds = {stoper.Elapsed.TotalSeconds} - New Transactions = {pool.Transactions.Count}/{syncingBlocks.CurrentPoolSyncing.Count()}");

            this.Runner.Get<BlockSyncer>().Enqueue(new SyncBlockOperation { PoolTransactions = pool });

            return await Task.FromResult(false);
        }
    }
}
