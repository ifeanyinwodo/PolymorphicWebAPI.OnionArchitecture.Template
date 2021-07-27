using AutoMapper;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Domain.Types;
using PolymorphicWebAPI.Persistence.Factories;
using PolymorphicWebAPI.Persistence.Repositories.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Persistence.Repositories
{
    public class StoreOrmFactory
    {
        private readonly Dictionary<string, IStoreOrm>  orm = new Dictionary<string, IStoreOrm>();
        
        public StoreOrmFactory( IMapper mapper, DatabaseConfig config)
        {

            IMapper _mapper = mapper;
            DatabaseConfig _config = config;
            orm.Add(OrmTypes.Dapper, new DapperStoreOrm(_mapper, _config));
            orm.Add(OrmTypes.Entityframework, new EntityFrameworkStoreOrm( _mapper, _config));
            orm.Add(OrmTypes.NHibernate, new NHibernateStoreOrm( _mapper, _config));
        }
        public IStoreOrm Create(string ormType)
        {
            return orm[ormType];
        }
    }
}
