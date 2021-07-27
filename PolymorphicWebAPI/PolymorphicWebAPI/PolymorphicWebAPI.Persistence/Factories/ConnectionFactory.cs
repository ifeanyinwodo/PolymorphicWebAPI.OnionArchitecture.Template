using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Domain.Types;
using Microsoft.Data.SqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Persistence.Factories
{
    public class ConnectionFactory 
    {
        private readonly Dictionary<string, GenericConnectionFactory> conectionfactory = new Dictionary<string, GenericConnectionFactory>();
        
        public ConnectionFactory(DatabaseConfig config)
        {
            DatabaseConfig _config = config;
            conectionfactory.Add(DataBaseTypes.PostgreSQL, new PgSqlConnectionFactory(_config));
            conectionfactory.Add(DataBaseTypes.MSSQLServer, new MSSqlConnectionFactory(_config));
        }

        public GenericConnectionFactory Create(string dbType)
        {
            return conectionfactory[dbType];
        }
    }
}
