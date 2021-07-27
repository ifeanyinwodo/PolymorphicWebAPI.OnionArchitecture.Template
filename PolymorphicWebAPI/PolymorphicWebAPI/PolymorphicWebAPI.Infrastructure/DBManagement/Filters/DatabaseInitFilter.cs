using Microsoft.AspNetCore.Hosting;
using System;
using DbUp;
using Microsoft.AspNetCore.Builder;
using PolymorphicWebAPI.Infrastructure.DBManagement.Logger;
using System.Reflection;
using PolymorphicWebAPI.Domain.Entities;
using DbUp.Builder;
using PolymorphicWebAPI.Domain.Types;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace PolymorphicWebAPI.Infrastructure.DBManagement.Filters
{
    public class DatabaseInitFilter : IStartupFilter
    {
        private readonly DatabaseConfig _config;
      
        private readonly string _ConnectionString;
        private readonly DbLogger<DatabaseInitFilter> _logger;

        public DatabaseInitFilter(DatabaseConfig config, DbLogger<DatabaseInitFilter> logger, IOptions<DatabaseConfig> optionsDBConfig )
        {
            _config = optionsDBConfig.Value;
           
            _logger = logger;
            _ConnectionString = _config.ConnectionString;


        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            UpgradeEngineBuilder dbUpgradeEngineBuilder = null;
        

            switch (_config.DataBaseType.ToLower()) {
                case DataBaseTypes.PostgreSQL:
                EnsureDatabase.For.PostgresqlDatabase(_ConnectionString);
                    dbUpgradeEngineBuilder = DeployChanges.To
                    .PostgresqlDatabase(_ConnectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .WithTransaction()
                    .LogToConsole();
                    break;        
                default:
                    EnsureDatabase.For.SqlDatabase(_ConnectionString);
                    dbUpgradeEngineBuilder = DeployChanges.To
                    .SqlDatabase(_ConnectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .WithTransaction()
                    .LogToConsole();
                    break;
            }

           
               
                
                
                var dbUpgradeEngine = dbUpgradeEngineBuilder.Build();

                if (dbUpgradeEngine.IsUpgradeRequired())
                {
                   
                    _logger.WriteInformation("Upgrades have been detected. Upgrading database now...");
                    var operation = dbUpgradeEngine.PerformUpgrade();
                    if (operation.Successful)
                    {
                        
                        _logger.WriteInformation("Upgrades have been detected. Upgrading database now...");
                    }

                    _logger.WriteInformation("Error happened in the upgrade. Please check the logs");
                    
                }
               
           
            return next;
        }
    }
}