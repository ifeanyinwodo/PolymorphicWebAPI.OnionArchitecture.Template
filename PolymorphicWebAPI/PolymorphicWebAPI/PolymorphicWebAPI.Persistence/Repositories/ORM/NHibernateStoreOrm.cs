

using AutoMapper;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Persistence.Exceptions;
using PolymorphicWebAPI.Persistence.Factories;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tactical.DDD;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using Domain.DomainEvents;

namespace PolymorphicWebAPI.Persistence.Repositories.ORM
{
    public class NHibernateStoreOrm : IStoreOrm
    {
        private readonly ISessionFactory _sessionFactory;    
        private readonly IMapper _mapper;
        
        public NHibernateStoreOrm( IMapper mapper, DatabaseConfig config) 
        {
            DatabaseConfig _config = config;
            ConnectionFactory _connectionFactory = new ConnectionFactory(_config);
            _mapper = mapper;
            _sessionFactory = FluentNHibernateSessionFactory.BuildSessionFactory(_connectionFactory.Create(_config.DataBaseType).ConnectionString); 
        }

        public static ISessionFactory BuildSessionFactory(string connectionStringName, bool create = false, bool update = false)
        {
            return Fluently.Configure()
            .Database(PostgreSQLConfiguration.PostgreSQL82
            .ConnectionString(connectionStringName))
            .Mappings(m => m.FluentMappings.Add<EventStoreDBSetNHibernateMap>())
            .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
            .BuildSessionFactory();
        }


       
        private static void BuildSchema(NHibernate.Cfg.Configuration config, bool create = false, bool update = false)
        {
            if (create)
            {
                new SchemaExport(config).Create(false, true);
            }
            else
            {
                new SchemaUpdate(config).Execute(false, update);
            }
        }

        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore
        };

        private IDomainEvent TransformEvent(EventStoreDBSet eventSelected)
        {
            
            var o = JsonConvert.DeserializeObject<DomainEvent>(eventSelected.Data, _jsonSerializerSettings);
            var evt = o as IDomainEvent;

            return evt;
        }

        public async Task<IQueryable<ItemCategoryDto>> EventSourcingLoadAllAsync()
        {
           

            using (var session = _sessionFactory.OpenSession())
            {
                
                
                using (var tx = session.BeginTransaction())
                {
                   
                    var events = await session.Query<EventStoreDBSet>().ToListAsync();
                  
                    var itemevents = events.ConvertAll(x => new List<IDomainEvent>() { TransformEvent(x) }.AsReadOnly());
                    
                    var itemsresult = itemevents.ConvertAll(x => _mapper.Map<ItemCategoryDto>(new ItemCategory(x)));
                    
                    tx.Commit();
                    return itemsresult.AsQueryable();

                    

                }
            }
        }

        public async Task<IQueryable<ItemCategoryDto>> EventSourcingLoadAsync(IEntityId aggregateRootId)
        {
            
            if (aggregateRootId == null) throw new AggregateRootNotProvidedException("AggregateRootId cannot be null");


            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {


                var events = await(from c in session.Query<EventStoreDBSet>() where c.AggregateId == aggregateRootId.ToString() select c).ToListAsync();              

                var itemevents = events.ConvertAll(x => new List<IDomainEvent>() { TransformEvent(x) }.AsReadOnly());

                var itemsresult = itemevents.ConvertAll(x => _mapper.Map<ItemCategoryDto>(new ItemCategory(x)));
                tx.Commit();
                return itemsresult.AsQueryable();

            }
        }

        public async Task<ItemCategory> EventSourcingLoadEventAsync(IEntityId aggregateRootId)
        {
           
            if (aggregateRootId == null) throw new AggregateRootNotProvidedException("AggregateRootId cannot be null");

            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {


                var events = await (from c in session.Query<EventStoreDBSet>() where c.AggregateId == aggregateRootId.ToString() select c).ToListAsync();
                var domainEvents = events.Select(TransformEvent).Where(x => x != null).ToList().AsReadOnly();
                var itemCategory = new ItemCategory(domainEvents);
                tx.Commit();

                return itemCategory;
                
            }
        }

        public async Task EventSourcingSaveAsync(ItemCategory itemCategory)
        {
            
            if (itemCategory.DomainEvents.Count == 0) return;

            int originatingVersion=itemCategory.Version;
            
            var listOfEvents = itemCategory.DomainEvents.Select(ev => new EventStoreDBSet
            {
               
                CreatedAt = ev.CreatedAt,
                Data = JsonConvert.SerializeObject(ev, Formatting.Indented, _jsonSerializerSettings),
                Id = Guid.NewGuid(),
                Name = ev.GetType().Name,
                AggregateId = itemCategory.Id.ToString(),
                Version = ++originatingVersion
            });
            
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
               
                await session.SaveAsync(listOfEvents.FirstOrDefault());
               
                tx.Commit();
               

            }

        }

        public async Task<ItemCategoryDto> EventSourcingSaveWithResultAsync(ItemCategory itemCategory)
        {
            
            if (itemCategory.DomainEvents.Count == 0) return null;
            
            int originatingVersion=itemCategory.Version;
            var listOfEvents = itemCategory.DomainEvents.Select(ev => new EventStoreDBSet
            {
                
                CreatedAt = ev.CreatedAt,
                Data = JsonConvert.SerializeObject(ev, Formatting.Indented, _jsonSerializerSettings),
                Id = Guid.NewGuid(),
                Name = ev.GetType().Name,
                AggregateId = itemCategory.Id.ToString(),
                Version = ++originatingVersion
            });

           

            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                
                await session.PersistAsync(listOfEvents);
                await session.SaveOrUpdateAsync(listOfEvents);
                var ItemCategoryDto = _mapper.Map<ItemCategoryDto>(new ItemCategory(itemCategory.DomainEvents));
                
                tx.Commit();
                return ItemCategoryDto;

            }

        }

       
        public async Task<IQueryable<ItemCategoryDto>> NonEventSourcingLoadAllAsync()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var items =await session.Query<ItemCategoryStoreDBSet>().ToListAsync(); 
                var selectedItem = _mapper.Map<List<ItemCategoryStoreDBSet>, List<ItemCategoryDto>>(items);

                tx.Commit();
                return selectedItem.AsQueryable();

            }
        }

        public async Task<IQueryable<ItemCategoryDto>> NonEventSourcingLoadAsync(IEntityId aggregateRootId)
        {
            if (aggregateRootId == null) throw new IdException("Id cannot be null");
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                
                var items =await (from c in session.Query<ItemCategoryStoreDBSet>() where c.Id == Guid.Parse(aggregateRootId.ToString()) select c).ToListAsync();
                 
                var selectedItem = _mapper.Map<List<ItemCategoryStoreDBSet>, List<ItemCategoryDto>>(items);
                tx.Commit();
                return selectedItem.AsQueryable();
               
            }
        }

        public async Task NonEventSourcingRemove(string id)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var iQueryitemcategory = session.Get<ItemCategoryStoreDBSet>(id);
            await session.DeleteAsync(iQueryitemcategory);
                tx.Commit();
            }
        }

        public async Task NonEventSourcingSaveAsync(ItemCategoryDto ItemCategoryDto)
        {
            
            if (ItemCategoryDto == null) return;
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                await session.SaveOrUpdateAsync(new ItemCategoryStoreDBSet { Id = Guid.Parse(ItemCategoryDto.Id), CategoryName = ItemCategoryDto.CategoryName, Description = ItemCategoryDto.Description, Quantity = ItemCategoryDto.Quantity });
                tx.Commit();
            }
        }

        public async Task<ItemCategoryDto> NonEventSourcingSaveUpdate(string id, string categoryName, string description, int quantity)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
               
                var itemcategory = session.Get<ItemCategoryStoreDBSet>(id);
                itemcategory.CategoryName = categoryName;
                itemcategory.Description = description;
                itemcategory.Quantity = quantity;
                await session.SaveOrUpdateAsync(itemcategory);
                tx.Commit();
                return new ItemCategoryDto { Id = id, CategoryName = categoryName, Description = description, Quantity = quantity };

            }
        }

        public async Task<ItemCategoryDto> NonEventSourcingSaveWithResultAsync(ItemCategoryDto ItemCategoryDto)
        {
            
            if (ItemCategoryDto == null) return null;
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                await session.SaveOrUpdateAsync(new ItemCategoryStoreDBSet { Id = Guid.Parse(ItemCategoryDto.Id), CategoryName = ItemCategoryDto.CategoryName, Description = ItemCategoryDto.Description, Quantity = ItemCategoryDto.Quantity });
                tx.Commit();
            }
            return ItemCategoryDto;

        }
    }
}
