// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockReorger.cs" company="SoftChains">
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

    using System.Threading.Tasks;

    using Nako.Config;
    using Nako.Operations;
    using Nako.Operations.Types;

    #endregion

    /// <summary>
    /// The block re-org of the block chain.
    /// </summary>
    public class BlockReorger : TaskStarter
    {
        /// <summary>
        /// The tracer.
        /// </summary>
        private readonly Tracer tracer;

        /// <summary>
        /// The operations.
        /// </summary>
        private readonly ISyncOperations operations;

        /// <summary>
        /// The connection.
        /// </summary>
        private readonly SyncConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockReorger"/> class.
        /// </summary>
        /// <param name="tracer">
        /// The tracer.
        /// </param>
        /// <param name="syncOperations">
        /// The sync operations.
        /// </param>
        /// <param name="syncConnection">
        /// The sync connection.
        /// </param>
        public BlockReorger(Tracer tracer, ISyncOperations syncOperations, SyncConnection syncConnection)
            : base(tracer)
        {
            this.connection = syncConnection;
            this.operations = syncOperations;
            this.tracer = tracer;
        }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        public override int Priority
        {
            get
            {
                return 50;
            }
        }

        /// <summary>
        /// The on execute.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task OnExecute()
        {
            this.tracer.Trace("BlockReorger", "Checking if re-org is required");

            return this.operations.CheckBlockReorganization(this.connection);
        }
    }
}
