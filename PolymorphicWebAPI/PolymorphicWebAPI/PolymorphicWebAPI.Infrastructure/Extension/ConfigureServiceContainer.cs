using AutoMapper;
using Confluent.Kafka;
using GreenPipes;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Domain.Types;
using PolymorphicWebAPI.Infrastructure.DBManagement.Filters;
using PolymorphicWebAPI.Infrastructure.DBManagement.Logger;
using PolymorphicWebAPI.Persistence.Factories;
using PolymorphicWebAPI.Persistence.Repositories;
using PolymorphicWebAPI.Persistence.Repositories.ORM;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using GraphQL.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using PolymorphicWebAPI.Service.Features.GraphQL;
using System.IO;
using PolymorphicWebAPI.Service.Features.MessageBroker;
using PolymorphicWebAPI.Domain.DTO;


namespace PolymorphicWebAPI.Infrastructure.Extension
{
    public static class ConfigureServiceContainer
    {
      
        public static void AddGraphQLServices(this IServiceCollection serviceCollection, IWebHostEnvironment environment)
        {
            
            serviceCollection
               .AddSingleton<ItemCategorySchema>()
               .AddGraphQL((options, provider) =>
               {
                   options.EnableMetrics = environment.IsDevelopment();
                   options.UnhandledExceptionDelegate = ctx => ctx.ErrorMessage = ctx.OriginalException.Message;
               })
               .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { })
               .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = environment.IsDevelopment())
               .AddWebSockets()
               .AddDataLoader()
               .AddGraphTypes(typeof(ItemCategorySchema));

        }

        public static void AddCacheServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("CacheOptions:ConnectionString");
            });
        }
        public static void AddScopedServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {

            serviceCollection.AddScoped<IStoreOrm, DapperStoreOrm>();
            serviceCollection.AddScoped<IStoreOrm, EntityFrameworkStoreOrm>();
            serviceCollection.AddScoped<IStoreOrm, NHibernateStoreOrm>();
            serviceCollection.AddScoped<IEventAndNoneEventStoreOrmRepository, EventStoreOrmRepository>();
            serviceCollection.AddScoped<IEventAndNoneEventStoreOrmRepository, NonEventStoreOrmRepository>();
            serviceCollection.AddSingleton<IStoreOrmRepository, StoreOrmRepository>();
           
        }


        public static void AddSingletonServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
           

            serviceCollection.AddSingleton<IStartupFilter, DatabaseInitFilter>();
            serviceCollection.AddDbContext<EntityFrameworkDatabaseContext>();
            serviceCollection.Configure<DatabaseConfig>(configuration.GetSection("DatabaseConfig"));
            serviceCollection.AddSingleton(provider =>
            {
                var configValue = provider.GetRequiredService<IOptions<DatabaseConfig>>().Value;
                
                return configValue;
            });
            serviceCollection.Configure<MessageQueueOptions>(configuration.GetSection("MessageQueueOptions"));
            serviceCollection.AddSingleton(provider =>
            {
                var configValue = provider.GetRequiredService<IOptions<MessageQueueOptions>>().Value;

                return configValue;
            });
            serviceCollection.Configure<CacheOptions>(configuration.GetSection("CacheOptions"));
            serviceCollection.AddSingleton(provider =>
            {
                var configValue = provider.GetRequiredService<IOptions<CacheOptions>>().Value;

                return configValue;
            });
        }


        

        public static void AddTransientServices(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddTransient<DbLogger<DatabaseInitFilter>>();
            serviceCollection.AddTransient<GenericConnectionFactory, PgSqlConnectionFactory>();

        }
        
        public static void AddSwaggerOpenAPI(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PolymorphicWebAPI", Version = "v1" });
            });

        }

        public static void AddController(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllers();

        }

        public static void AddMapper(this IServiceCollection serviceCollection)
        {
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new ItemCategoryProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);

        }


        public static void AddVersion(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }




    }
}
