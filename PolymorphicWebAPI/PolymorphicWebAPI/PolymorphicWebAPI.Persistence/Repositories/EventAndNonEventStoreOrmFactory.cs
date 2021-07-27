using AutoMapper;
using PolymorphicWebAPI.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Persistence.Repositories
{
    public class EventAndNonEventStoreOrmFactory
    {
        private readonly Dictionary<string, IEventAndNoneEventStoreOrmRepository> eventandnoneventorm = new Dictionary<string, IEventAndNoneEventStoreOrmRepository>();
        
        public EventAndNonEventStoreOrmFactory(IStoreOrmRepository storeORMRepository, IMapper mapper)
         {
           
            IMapper _mapper = mapper;
            IStoreOrmRepository _storeORMRepository = storeORMRepository;
            eventandnoneventorm.Add(StoreTypes.Eventsourcing, new EventStoreOrmRepository(_storeORMRepository, _mapper));
            eventandnoneventorm.Add(StoreTypes.NonEventsourcing, new NonEventStoreOrmRepository(_storeORMRepository, _mapper));
        }

        public IEventAndNoneEventStoreOrmRepository Create(string ormType)
        {
            return eventandnoneventorm[ormType];
        }
    }
}
