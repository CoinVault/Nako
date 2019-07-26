// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Runner.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Sync.SyncTasks
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Nako.Client.Types;
    using Nako.Extensions;
    using Nako.Operations.Types;

    /// <summary>
    /// The runner.
    /// </summary>
    public class Runner
    {
        /// <summary>
        /// The task runners.
        /// </summary>
        private readonly IEnumerable<TaskRunner> taskRunners;

        /// <summary>
        /// The task starters.
        /// </summary>
        private readonly IEnumerable<TaskStarter> taskStarters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Runner"/> class.
        /// </summary>
        public Runner(IEnumerable<TaskStarter> taskStarters, IEnumerable<TaskRunner> taskRunners)
        {
            this.taskStarters = taskStarters;
            this.taskRunners = taskRunners;
            this.SyncingBlocks = new SyncingBlocks { CurrentSyncing = new ConcurrentDictionary<string, BlockInfo>(), CurrentPoolSyncing = new List<string>() };
        }

        public SyncingBlocks SyncingBlocks { get; set; }

        public T Get<T>() where T : TaskRunner
        {
            return this.taskRunners.OfType<T>().Single();
        }

        public void BlockAndDeplete()
        {
            this.SyncingBlocks.Blocked = true;

            this.taskRunners.OfType<IBlockableItem>().ForEach(f => f.Blocked = true);
            this.taskRunners.OfType<IBlockableItem>().ForEach(f => f.Deplete());

            this.SyncingBlocks.LastBlock = null;
            this.SyncingBlocks.CurrentPoolSyncing.Clear();
            this.SyncingBlocks.CurrentSyncing.Clear();
        }

        public void UnBlock()
        {
            this.taskRunners.OfType<IBlockableItem>().ForEach(f => f.Blocked = false);
            this.SyncingBlocks.Blocked = false;
        }

        /// <summary>
        /// Run all tasks.
        /// </summary>
        public IEnumerable<Task> RunAll(CancellationTokenSource cancellationToken)
        {
            // execute all the starters sequentially
            this.taskStarters.OrderBy(o => o.Priority).ForEach(t => t.Run(this, cancellationToken).Wait());

            // execute the tasks
            return this.taskRunners.Select(async t => await t.Run(this, cancellationToken));
        }
    }
}
