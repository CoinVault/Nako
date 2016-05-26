// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskRunner{T}.cs" company="SoftChains">
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
    using System.Collections.Concurrent;
    using System.Linq;

    using Nako.Config;

    #endregion

    /// <summary>
    /// The task runner.
    /// </summary>
    /// <typeparam name="T">
    /// The queue type.
    /// </typeparam>
    public abstract class TaskRunner<T> : TaskRunner, IBlockableItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRunner{T}"/> class.
        /// </summary>
        /// <param name="application">
        /// The application.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <param name="tracer">
        /// The tracer.
        /// </param>
        protected TaskRunner(NakoApplication application, NakoConfiguration config, Tracer tracer)
            : base(application, config, tracer)
        {
            this.Queue = new ConcurrentQueue<T>();
        }

        /// <summary>
        /// Gets or sets the queue.
        /// </summary>
        public ConcurrentQueue<T> Queue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the queue is blocked.
        /// </summary>
        public bool Blocked { get; set; }

        /// <summary>
        /// The en-queue.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Enqueue(T item)
        {
            if (!this.Blocked)
            {
                this.Queue.Enqueue(item);
            }
        }

        /// <summary>
        /// The en-queue.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool TryDequeue(out T result)
        {
            if (!this.Blocked)
            {
                return this.Queue.TryDequeue(out result);
            }

            result = default(T);
            return false;
        }

        /// <summary>
        /// The Deplete all items form queue.
        /// </summary>
        public void Deplete()
        {
            T item;
            while (this.Queue.TryDequeue(out item))
            {
                // Do nothing.
            }

            if (this.Queue.Any())
            {
                throw new ApplicationException("Failed to empty queue.");
            }
        }
    }
}