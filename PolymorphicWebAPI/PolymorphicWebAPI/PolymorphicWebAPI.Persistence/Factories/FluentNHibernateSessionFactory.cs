using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using PolymorphicWebAPI.Domain.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Persistence.Factories
{
    public static  class FluentNHibernateSessionFactory
    {

       
        public static ISessionFactory BuildSessionFactory(string connectionStringName)
        {
            return Fluently.Configure()
            .Database(PostgreSQLConfiguration.PostgreSQL82
            .ConnectionString(connectionStringName))
            .Mappings(m =>m.FluentMappings.Add<EventStoreDBSetNHibernateMap>() )
            .CurrentSessionContext("call")
            .BuildSessionFactory();
        }


        
    }
}
