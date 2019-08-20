// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockStore.cs" company="SoftChains">
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
    using Nako.Client.Types;
    using Nako.Config;
    using Nako.Extensions;
    using Nako.Operations;
    using Nako.Operations.Types;

    /// <summary>
    /// The block sync.
    /// </summary>
    public class BlockStore : TaskRunner<SyncBlockTransactionsOperation>
    {
        private readonly ILogger<BlockStore> log;

        private readonly IStorageOperations storageOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockStore"/> class.
        /// </summary>
        public BlockStore(IOptions<NakoConfiguration> configuration, ILogger<BlockStore> logger, IStorageOperations storageOperations)
            : base(configuration, logger)
        {
            this.storageOperations = storageOperations;
            this.log = logger;
        }

        /// <inheritdoc />
        public override async Task<bool> OnExecute()
        {
            SyncBlockTransactionsOperation item;

            if (this.TryDequeue(out item))
            {
                var stoper = Stopwatch.Start();

                this.storageOperations.ValidateBlock(item);

                var count = this.storageOperations.InsertTransactions(item);
 
                if (item.BlockInfo != null)
                {
                    BlockInfo blockInfo;
                    if (!this.Runner.SyncingBlocks.CurrentSyncing.TryRemove(item.BlockInfo.Hash, out blockInfo))
                    {
                        throw new Exception(string.Format("Failed to remove block hash {0} from collection", item.BlockInfo.Hash));
                    }
                }

                var notifications = new AddressNotifications { Addresses = count.Items.Where( ad => ad.Addresses != null).SelectMany(s => s.Addresses).Distinct().ToList() };
                this.Runner.Get<Notifier>().Enqueue(notifications);

                stoper.Stop();

                var message = item.BlockInfo != null ? 
                    string.Format("Seconds = {0} - BlockIndex = {1} - TotalItems = {2}", stoper.Elapsed.TotalSeconds, item.BlockInfo.Height, count.Transactions + count.Inputs + count.Outputs) :
                    string.Format("Seconds = {0} - PoolSync - TotalItems = {1}", stoper.Elapsed.TotalSeconds, count.Transactions + count.Inputs + count.Outputs);

                this.log.LogDebug(message);

                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
