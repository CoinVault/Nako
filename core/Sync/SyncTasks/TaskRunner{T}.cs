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

    public abstract class TaskRunner<T> : TaskRunner, IBlockableItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRunner{T}"/> class.
        /// </summary>
        protected TaskRunner(NakoApplication application, NakoConfiguration config, Tracer tracer)
            : base(application, config, tracer)
        {
            this.Queue = new ConcurrentQueue<T>();
        }

        public ConcurrentQueue<T> Queue { get; set; }

        public bool Blocked { get; set; }

        public void Enqueue(T item)
        {
            if (!this.Blocked)
            {
                this.Queue.Enqueue(item);
            }
        }

        public bool TryDequeue(out T result)
        {
            if (!this.Blocked)
            {
                return this.Queue.TryDequeue(out result);
            }

            result = default(T);
            return false;
        }

        public void Deplete()
        {
            T item;
            while (this.Queue.TryDequeue(out item))
            {
                // Do nothing.
            }

            if (this.Queue.Any())
            {
                throw new Exception("Failed to empty queue.");
            }
        }
    }
}