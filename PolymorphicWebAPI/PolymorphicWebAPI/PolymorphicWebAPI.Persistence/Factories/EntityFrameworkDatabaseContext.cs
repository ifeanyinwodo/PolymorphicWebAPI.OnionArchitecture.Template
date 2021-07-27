using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Domain.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Persistence.Factories
{
   
    public class EntityFrameworkDatabaseContext : DbContext
    {
        private readonly string _connectionString;
        private readonly DatabaseConfig _config;
        public DbSet<EventStoreDBSet> EventStore { get; set; }
        public DbSet<ItemCategoryStoreDBSet> ItemCategoryStoreDBSet { get; set; }



        public EntityFrameworkDatabaseContext(DatabaseConfig config)
        {
            _config = config;
            _connectionString = _config.ConnectionString;
         }

       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (_config.DataBaseType.ToLower())
            {
                case DataBaseTypes.PostgreSQL:
                    optionsBuilder.UseNpgsql(_connectionString);
                    break;
                case DataBaseTypes.MSSQLServer:
                    optionsBuilder.UseSqlServer(_connectionString);
                    break;
            }
            
           
            base.OnConfiguring(optionsBuilder);
        }

    }
}
