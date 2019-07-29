// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Nako.Api;
using Nako.Api.Handlers;
using Nako.Config;
using Nako.Operations;
using Nako.Operations.Types;
using Nako.Storage;
using Nako.Storage.Mongo;
using Nako.Sync;
using Nako.Sync.SyncTasks;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace Nako
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<NakoConfiguration>(Configuration);

            services.AddSingleton<QueryHandler>();
            services.AddSingleton<StatsHandler>();
            services.AddSingleton<CommandHandler>();
            services.AddSingleton<IStorage, MongoData>();
            services.AddSingleton<IStorageOperations, MongoStorageOperations>();
            services.AddSingleton<TaskStarter, MongoBuilder>();
            services.AddTransient<SyncServer>();
            services.AddSingleton<SyncConnection>();
            services.AddSingleton<ISyncOperations, SyncOperations>();
            services.AddScoped<Runner>();
            services.AddScoped<TaskRunner, BlockFinder>();
            services.AddScoped<TaskRunner, BlockStore>();
            services.AddScoped<TaskRunner, BlockSyncer>();
            services.AddScoped<TaskRunner, PoolFinder>();
            services.AddScoped<TaskRunner, Notifier>();
            services.AddScoped<TaskStarter, BlockReorger>();

            services.AddHostedService<SyncServer>();

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                });

            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                });

            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'V";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.AssumeDefaultVersionWhenUnspecified = true;

                    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);

                    options.SubstituteApiVersionInUrl = false;
                });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    // integrate xml comments
                    options.IncludeXmlComments(XmlCommentsFilePath);

                    options.DescribeAllEnumsAsStrings();

                    options.DescribeStringEnumsInCamelCase();
                });
        }

        public void Configure(
          IApplicationBuilder app,
          IApplicationLifetime appLifetime,
          IApiVersionDescriptionProvider provider)
        {
            app.UseCors(builder => builder.AllowAnyOrigin());

            app.UseMvc();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(
              options =>
              {
                  // build a swagger endpoint for each discovered API version
                  foreach (var description in provider.ApiVersionDescriptions)
                  {
                      options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                  }
              });

            app.UseSpa(builder => builder.Options.DefaultPage = "/index.html");
        }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }
    }
}
