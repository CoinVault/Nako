// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockFinder.cs" company="SoftChains">
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

    /// <summary>
    /// The block sync.
    /// </summary>
    public class BlockFinder : TaskRunner
    {
        private readonly NakoConfiguration config;

        private readonly ISyncOperations syncOperations;

        private readonly SyncConnection syncConnection;

        private readonly ILogger<BlockFinder> log;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockFinder"/> class.
        /// </summary>
        public BlockFinder(IOptions<NakoConfiguration> configuration, ISyncOperations syncOperations, SyncConnection syncConnection, ILogger<BlockFinder> logger)
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
            if (!this.config.SyncBlockchain)
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

            var block = this.syncOperations.FindBlock(this.syncConnection, syncingBlocks);

            if (block == null || block.BlockInfo == null)
            {
                return false;
            }

            stoper.Stop();

            var blockStatus = block.IncompleteBlock ? "Incomplete" : string.Empty;
            this.log.LogDebug($"Seconds = {stoper.Elapsed.TotalSeconds} - SyncedIndex = {block.BlockInfo.Height}/{block.LastCryptoBlockIndex} - {block.LastCryptoBlockIndex - block.BlockInfo.Height} {blockStatus}");

            this.Runner.Get<BlockSyncer>().Enqueue(block);

            return await Task.FromResult(true);
        }
    }
}
