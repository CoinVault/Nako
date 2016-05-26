// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageModule.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Storage
{
    #region Using Directives

    using Autofac;

    using Nako.Operations;
    using Nako.Storage.Mongo;
    using Nako.Sync.SyncTasks;

    #endregion

    /// <summary>
    /// The module.
    /// </summary>
    public class StorageModule : Module 
    {
        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MongoData>().As<IStorage>().AsSelf().SingleInstance();
            builder.RegisterType<MongoStorageOperations>().As<IStorageOperations>().SingleInstance();
            builder.RegisterType<MongoBuilder>().As<TaskStarter>().SingleInstance();

            ////builder.RegisterType<SqlData>().As<IStorage>().AsSelf().SingleInstance();
            ////builder.RegisterType<StorageOperations>().As<IStorageOperations>().SingleInstance();
        }
    }
}