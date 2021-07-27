
using PolymorphicWebAPI.Infrastructure.Extension;
using PolymorphicWebAPI.Service.Configs;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using PolymorphicWebAPI.Service.Features.GRPC;
using PolymorphicWebAPI.Service.Features.Worker;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PolymorphicWebAPI
{
    public class Startup
    {
       
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            
            Configuration = configuration;
            Environment = environment;

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = configBuilder.Build();



        }



        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddGraphQLServices(Environment);
            services.AddServiceLayer();
            services.AddFeatureManagement();
            services.AddController();
            services.AddSingletonServices(Configuration);
            services.AddScopedServices(Configuration);
            services.AddTransientServices();
            services.AddVersion();
            services.AddMapper();
            services.AddCacheServices(Configuration);
            services.AddSwaggerOpenAPI();
           

            if (Configuration.GetValue<bool>("MessageQueueOptions:Enable") && Configuration.GetValue<string>("MessageQueueOptions:Type").ToLower() == "consumer")
                services.AddHostedService<MQWorker>();
            services.AddMetrics();
            services.AddAppMetricsCollectors();
            services.AddAppMetricsHealthPublishing();
            services.AddMetricsTrackingMiddleware();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.ConfigureSwagger();

            }


            app.ConfigureCustomExceptionMiddleware();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseWebSockets();
            app.ConfigureGraphQL();
            app.UseMetricsRequestTrackingMiddleware();
            app.UseMetricsPostAndPutSizeTrackingMiddleware();
            app.UseMetricsErrorTrackingMiddleware();
            app.UseMetricsAllMiddleware(); 
            app.UseGraphQLPlayground();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<ItemCategoriesService>();
                endpoints.MapControllers();
               

            });
        }
    }
}
