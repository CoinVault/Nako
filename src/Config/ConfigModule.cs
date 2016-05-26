// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigModule.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Config
{
    #region Using Directives

    using Autofac;

    #endregion

    /// <summary>
    /// The module.
    /// </summary>
    public class ConfigModule : Module 
    {
        /// <summary>
        /// The load.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NakoApplication>().SingleInstance();
            builder.RegisterType<Terminator>().SingleInstance();
            builder.RegisterType<Tracer>().SingleInstance();
        }
    }
}