// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncModule.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Sync
{
    #region Using Directives

    using Autofac;

    using Nako.Operations;
    using Nako.Operations.Types;
    using Nako.Sync.SyncTasks;

    #endregion

    /// <summary>
    /// The module.
    /// </summary>
    public class SyncModule : Module 
    {
        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        protected override void Load(ContainerBuilder builder)
        {
            // server
            builder.RegisterType<SyncServer>();
        
            // operations
            builder.RegisterType<SyncConnection>().SingleInstance();
            builder.RegisterType<SyncOperations>().As<ISyncOperations>().SingleInstance();

            // runners
            builder.RegisterType<Runner>().InstancePerLifetimeScope();
            builder.RegisterType<BlockFinder>().As<TaskRunner>().InstancePerLifetimeScope();
            builder.RegisterType<BlockStore>().As<TaskRunner>().InstancePerLifetimeScope();
            builder.RegisterType<BlockSyncer>().As<TaskRunner>().InstancePerLifetimeScope();
            builder.RegisterType<PoolFinder>().As<TaskRunner>().InstancePerLifetimeScope();
            builder.RegisterType<Notifier>().As<TaskRunner>().InstancePerLifetimeScope();
            builder.RegisterType<BlockReorger>().As<TaskStarter>().InstancePerLifetimeScope();
        }
    }
}