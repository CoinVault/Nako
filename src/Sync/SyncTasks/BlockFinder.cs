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
    #region Using Directives

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Nako.Config;
    using Nako.Extensions;
    using Nako.Operations;
    using Nako.Operations.Types;

    #endregion

    /// <summary>
    /// The block sync.
    /// </summary>
    public class BlockFinder : TaskRunner
    {
        /// <summary>
        /// The config.
        /// </summary>
        private readonly NakoConfiguration config;

        /// <summary>
        /// The sync operations.
        /// </summary>
        private readonly ISyncOperations syncOperations;

        /// <summary>
        /// The sync connection.
        /// </summary>
        private readonly SyncConnection syncConnection;

        /// <summary>
        /// The tracer.
        /// </summary>
        private readonly Tracer tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockFinder"/> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <param name="syncOperations">
        /// The sync operations.
        /// </param>
        /// <param name="syncConnection">
        /// The sync connection.
        /// </param>
        /// <param name="tracer">
        /// The tracer.
        /// </param>
        public BlockFinder(NakoApplication application, NakoConfiguration config, ISyncOperations syncOperations, SyncConnection syncConnection, Tracer tracer)
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

            var stoper = StopwatchExtension.CreateAndStart();

            var block = await this.syncOperations.FindBlock(this.syncConnection, syncingBlocks);

            if (block == null || block.BlockInfo == null)
            {
                return false;
            }

            stoper.Stop();

            this.tracer.Trace("BlockFinder", string.Format("Seconds = {0} - SyncedIndex = {1}/{2} - {3} {4}", stoper.Elapsed.TotalSeconds, block.BlockInfo.Height, block.LastCryptoBlockIndex, block.LastCryptoBlockIndex - block.BlockInfo.Height, block.IncompleteBlock ? "Incomplete" : string.Empty), ConsoleColor.DarkCyan);

            this.Runner.Get<BlockSyncer>().Enqueue(block);
            
            return true;
        }
    }
}
