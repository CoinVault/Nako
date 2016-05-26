// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiModule.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Api
{
    #region Using Directives

    using System.Reflection;

    using Autofac;
    using Autofac.Integration.WebApi;

    using Nako.Api.Handlers;

    using Module = Autofac.Module;

    #endregion

    public class ApiModule : Module 
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApiServer>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<QueryHandler>().SingleInstance();
            builder.RegisterType<StatsHandler>().SingleInstance();
        }
    }
}