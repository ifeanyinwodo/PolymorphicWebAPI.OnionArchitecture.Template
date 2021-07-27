using PolymorphicWebAPI.Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Persistence.Factories
{
    public class MSSqlConnectionFactory : GenericConnectionFactory
    {
        public override string ConnectionString { get; }
        

        public MSSqlConnectionFactory(DatabaseConfig config)
        {
            DatabaseConfig _config = config;
            ConnectionString = _config.ConnectionString;
        }

        public override SqlConnection DBConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        
    }
}
