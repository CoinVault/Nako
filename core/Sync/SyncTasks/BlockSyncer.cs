// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockSyncer.cs" company="SoftChains">
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
    using Nako.Client;
    using Nako.Config;
    using Nako.Extensions;
    using Nako.Operations;
    using Nako.Operations.Types;

    /// <summary>
    /// The block sync.
    /// </summary>
    public class BlockSyncer : TaskRunner<SyncBlockOperation>
    {
        private readonly NakoConfiguration config;

        private readonly ISyncOperations syncOperations;

        private readonly SyncConnection syncConnection;

        private readonly ILogger<BlockSyncer> log;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockSyncer"/> class.
        /// </summary>
        public BlockSyncer(IOptions<NakoConfiguration> configuration, ISyncOperations syncOperations, SyncConnection syncConnection, ILogger<BlockSyncer> logger)
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
            if (this.Runner.Get<BlockStore>().Queue.Count() >= this.config.MaxItemsInQueue)
            {
                return false;
            }

            SyncBlockOperation item;

            if (this.TryDequeue(out item))
            {
                var stoper = Stopwatch.Start();

                try
                {
                    if (item.BlockInfo != null)
                    {
                        var block = this.syncOperations.SyncBlock(this.syncConnection, item.BlockInfo);

                        var inputs = block.Transactions.SelectMany(s => s.VIn).Count();
                        var outputs = block.Transactions.SelectMany(s => s.VOut).Count();

                        this.Runner.Get<BlockStore>().Enqueue(block);

                        this.log.LogDebug($"Seconds = {stoper.Elapsed.TotalSeconds} - BlockIndex = {block.BlockInfo.Height} - Transactions {block.Transactions.Count()} - Inputs {inputs} - Outputs {outputs} - ({inputs + outputs})");
                    }

                    if (item.PoolTransactions != null)
                    {
                        var pool = this.syncOperations.SyncPool(this.syncConnection, item.PoolTransactions);

                        var inputs = pool.Transactions.SelectMany(s => s.VIn).Count();
                        var outputs = pool.Transactions.SelectMany(s => s.VOut).Count();

                        this.Runner.Get<BlockStore>().Enqueue(pool);

                        this.log.LogDebug($"Seconds = {stoper.Elapsed.TotalSeconds} - Pool = Sync - Transactions {pool.Transactions.Count()} - Inputs {inputs} - Outputs {outputs} - ({inputs + outputs})");
                    }
                }
                catch (BitcoinClientException bce)
                {
                    if (bce.ErrorCode == -5)
                    {
                        if (bce.ErrorMessage == "No information available about transaction")
                        {
                            throw new SyncRestartException(string.Empty, bce);
                        }
                    }

                    throw;
                }

                stoper.Stop();

                return true;
            }

            return await Task.FromResult(false);
        }
    }
}
