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
    #region Using Directives

    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Nako.Client.Types;
    using Nako.Extensions;
    using Nako.Operations.Types;

    #endregion

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
        /// The taskStarters.
        /// </summary>
        private readonly IEnumerable<TaskStarter> taskStarters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Runner"/> class.
        /// </summary>
        /// <param name="taskStarters">
        /// The task Starters.
        /// </param>
        /// <param name="taskRunners">
        /// The task runners.
        /// </param>
        public Runner(TaskStarter[] taskStarters, TaskRunner[] taskRunners)
        {
            this.taskStarters = taskStarters;
            this.taskRunners = taskRunners;
            this.SyncingBlocks = new SyncingBlocks { CurrentSyncing = new ConcurrentDictionary<string, BlockInfo>(), CurrentPoolSyncing = new List<string>() };
        }

        /// <summary>
        /// Gets or sets the blocks.
        /// </summary>
        public SyncingBlocks SyncingBlocks { get; set; }

        /// <summary>
        /// The get.
        /// </summary>
        /// <typeparam name="T">
        /// The runner type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Get<T>() where T : TaskRunner
        {
            return this.taskRunners.OfType<T>().Single();
        }

        /// <summary>
        /// The block and deplete.
        /// </summary>
        public void BlockAndDeplete()
        {
            this.SyncingBlocks.Blocked = true;

            this.taskRunners.OfType<IBlockableItem>().ForEach(f => f.Blocked = true);
            this.taskRunners.OfType<IBlockableItem>().ForEach(f => f.Deplete());

            this.SyncingBlocks.LastBlock = null;
            this.SyncingBlocks.CurrentPoolSyncing.Clear();
            this.SyncingBlocks.CurrentSyncing.Clear();
        }

        /// <summary>
        /// The un block.
        /// </summary>
        public void UnBlock()
        {
            this.taskRunners.OfType<IBlockableItem>().ForEach(f => f.Blocked = false);
            this.SyncingBlocks.Blocked = false;
        }

        /// <summary>
        /// The run all.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public IEnumerable<Task> RunAll(CancellationTokenSource cancellationToken)
        {
            // execute all the starters sequentially
            this.taskStarters.OrderBy(o => o.Priority).ForEach(t => t.Run(this, cancellationToken).Wait());

            // execute the tasks
            return this.taskRunners.Select(async t => await t.Run(this, cancellationToken));
        }
    }
}
