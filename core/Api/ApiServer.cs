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
    //using Autofac.Integration.WebApi;

    //using Microsoft.Owin.Hosting;

    using Nako.Config;

  //  using Owin;

//using Swashbuckle.Application;
    using Microsoft.AspNetCore.Hosting;
    using System.IO;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Builder;
    using Autofac.Extensions.DependencyInjection;

    #endregion

    /// <summary>
    /// A server to start an OWIN selfhost api.
    /// </summary>
    public class ApiServer
    {
        private readonly NakoApplication application;

        private readonly NakoConfiguration configuration;

        private readonly Tracer tracer;

        //// This code configures Web API. The ApiServer class is specified as a type
        //// parameter in the WebApp.Start method.
        public ApiServer(NakoApplication nakoApplication, NakoConfiguration nakoConfiguration, Tracer tracer)
        {
            this.configuration = nakoConfiguration;
            this.tracer = tracer;
            this.application = nakoApplication;
        }

        public void StartApi(IContainer container)
        {
            this.application.CreateApiToken();

            var url = string.Format("http://+:{0}", this.configuration.SyncApiPort);

            var host = new WebHostBuilder()
              .UseContentRoot(Directory.GetCurrentDirectory())
              .UseUrls(url)
              .UseKestrel()
              .UseStartup<ApiServer>()
              .Build();

            host.Run();

            //return Task.Run(
            //    () =>
            //    {
            //        try
            //        {
            //            var url = string.Format("http://+:{0}", this.configuration.SyncApiPort);
            //            var options = new StartOptions(url);

            //            using (WebApp.Start(options, appBuilder => this.Configuration(appBuilder, container)))
            //            {
            //                this.tracer.Trace("API", string.Format("Self host server running at {0}", url));

            //                Task.Delay(Timeout.Infinite, this.application.ApiToken).Wait(this.application.ApiToken);
            //            }
            //        }
            //        catch (OperationCanceledException)
            //        {
            //            // do nothing the task was cancel.
            //            throw;
            //        }
            //        catch (Exception ex)
            //        {
            //            this.tracer.TraceError("API", ex.ToString());
            //            //Console.ReadLine();
            //            throw;
            //        }
            //    }, 
            //    this.application.ApiToken);
        }

        // ConfigureServices is where you register dependencies. This gets
        // called by the runtime before the Configure method, below.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add services to the collection.
            services.AddMvc();

            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container. If you want
            // to dispose of the container at the end of the app,
            // be sure to keep a reference to it as a property or field.
            // builder.RegisterType<MyType>().As<IMyType>();
            //builder.Populate(services);
            //this.ApplicationContainer = builder.Build();
            var cointainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(cointainer);
        }

        // Configure is where you add middleware. This is called after
        // ConfigureServices. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(
          IApplicationBuilder app,
          IApplicationLifetime appLifetime)
        {
            app.UseMvc();
            
            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            //appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }

    ///// <summary>
    ///// Configuration of the OWIN api, setup swagger and autofac(DI).
    ///// </summary>
    //public void Configuration(IAppBuilder appBuilder, IContainer container)
    //    {
    //        // Configure Web API for self-host. 
    //        var config = new HttpConfiguration();
    //        config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

    //        config.MapHttpAttributeRoutes();
    //        config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

    //        config.EnableSwagger(c => c.SingleApiVersion("v1", "The Nako API")).EnableSwaggerUi();    

    //        appBuilder.UseAutofacMiddleware(container);
    //        appBuilder.UseAutofacWebApi(config);
    //        appBuilder.UseWebApi(config);
    //    }
    //} 
}
