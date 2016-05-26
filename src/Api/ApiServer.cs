// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiServer.cs" company="SoftChains">
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

    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Autofac;
    using Autofac.Integration.WebApi;

    using Microsoft.Owin.Hosting;

    using Nako.Config;

    using Owin;

    using Swashbuckle.Application;

    #endregion

    /// <summary>
    /// A server to start an OWIN selfhost api.
    /// </summary>
    public class ApiServer 
    {
        private readonly NakoApplication application;

        private readonly NakoConfiguration configuration;

        //// This code configures Web API. The ApiServer class is specified as a type
        //// parameter in the WebApp.Start method.
        public ApiServer(NakoApplication nakoApplication, NakoConfiguration nakoConfiguration)
        {
            this.configuration = nakoConfiguration;
            this.application = nakoApplication;
        }

        public Task StartApi(IContainer container)
        {
            this.application.CreateApiToken();

            return Task.Run(
                () =>
                {
                    try
                    {
                        var url = string.Format("http://+:{0}", this.configuration.SyncApiPort);
                        var options = new StartOptions(url);

                        options.Urls.ToList().ForEach(u => Console.WriteLine("Self host server running at {0}", u));

                        Console.WriteLine(@"Help: If url not working add to netsh - command = netsh http add urlacl url=http://+:{0}/ user=machine\username", this.configuration.SyncApiPort);

                        using (WebApp.Start(options, appBuilder => this.Configuration(appBuilder, container)))
                        {
                            Task.Delay(Timeout.Infinite, this.application.ApiToken).Wait(this.application.ApiToken);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // do nothing the task was cancel.
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Console.ReadLine();
                        throw;
                    }
                }, 
                this.application.ApiToken);
        }

        /// <summary>
        /// Configuration of the OWIN api, setup swagger and autofac(DI).
        /// </summary>
        public void Configuration(IAppBuilder appBuilder, IContainer container)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.MapHttpAttributeRoutes();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            config.EnableSwagger(c => c.SingleApiVersion("v1", "The Nako API")).EnableSwaggerUi();    

            appBuilder.UseAutofacMiddleware(container);
            appBuilder.UseAutofacWebApi(config);
            appBuilder.UseWebApi(config);
        }
    } 
}
