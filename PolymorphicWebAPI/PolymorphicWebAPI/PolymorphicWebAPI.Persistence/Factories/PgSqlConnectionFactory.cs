using PolymorphicWebAPI.Domain.Entities;
using Npgsql;


namespace PolymorphicWebAPI.Persistence.Factories
{
    public class PgSqlConnectionFactory : GenericConnectionFactory
    {
        public override string ConnectionString { get; }
        
        public PgSqlConnectionFactory( DatabaseConfig config)
        {
            DatabaseConfig _config = config;
            ConnectionString = _config.ConnectionString;
        }

       

        public override NpgsqlConnection DBConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

    }
}
