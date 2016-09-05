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
    using Swashbuckle.Swagger.Model;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Configuration;

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

        public Task StartApi(IContainer container)
        {
            this.application.CreateApiToken();

            return Task.Run(
                () =>
                {
                    try
                    {
                        var url = string.Format("http://+:{0}", this.configuration.SyncApiPort);

                        var hostbuilder = new WebHostBuilder()
                          .UseKestrel()
                          .UseContentRoot(Directory.GetCurrentDirectory())
                          .UseUrls(url)
                          .ConfigureServices(serv =>
                          {
                              serv.Add(new ServiceDescriptor(typeof(NakoConfiguration), container.Resolve<NakoConfiguration>()));
                          })
                          .UseStartup<Startup>();
                          
                        var host = hostbuilder.Build();
                        host.Start();

                        Task.Delay(Timeout.Infinite, this.application.ApiToken).Wait(this.application.ApiToken);
                    }
                    catch (OperationCanceledException)
                    {
                        // do nothing the task was cancel.
                        throw;
                    }
                    catch (Exception ex)
                    {
                        this.tracer.TraceError("API", ex.ToString());
                        //Console.ReadLine();
                        throw;
                    }
                },
                this.application.ApiToken);
        }

        public class Startup
        {
            public Startup(IHostingEnvironment env)
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath);
                Configuration = builder.Build();
            }

            public IConfigurationRoot Configuration { get; }

            // ConfigureServices is where you register dependencies. This gets
            // called by the runtime before the Configure method, below.
            public IServiceProvider ConfigureServices(IServiceCollection services)
            {
                services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                });

                services.AddSwaggerGen();
                services.ConfigureSwaggerGen(options =>
                {
                    options.SingleApiVersion(new Info
                    {
                        Version = "v1",
                        Title = "Nako API",
                    });
                    options.DescribeAllEnumsAsStrings();
                });
               
                   
                // Create the container builder.
                // asp.net core autofac builds its container at this stage, 
                // because the Nako server depends on the container being built already, 
                // we'll build a new service provider for the api calls.  
                // this means to register all the modules again in the api container (ugly hack but works for now)
                var builder = new ContainerBuilder();
                builder.RegisterAssemblyModules(System.Reflection.Assembly.GetEntryAssembly());
                builder.Populate(services);
                var cointainer = builder.Build();

                // Create the IServiceProvider based on the container.
                return new AutofacServiceProvider(cointainer);
            }

            public void Configure(
              IApplicationBuilder app,
              IApplicationLifetime appLifetime)
            {
                app.UseMvc();

                app.UseSwagger();
                app.UseSwaggerUi();
            }
        }
    }
}
