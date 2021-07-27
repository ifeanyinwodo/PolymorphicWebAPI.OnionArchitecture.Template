using AutoMapper;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Persistence.Exceptions;
using PolymorphicWebAPI.Persistence.Factories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tactical.DDD;
using Microsoft.EntityFrameworkCore;
using Domain.DomainEvents;

namespace PolymorphicWebAPI.Persistence.Repositories.ORM
{
    public class EntityFrameworkStoreOrm : IStoreOrm
    {

        private readonly EntityFrameworkDatabaseContext _entityFrameworkDatabaseContext;
        private readonly IMapper _mapper;
        public EntityFrameworkStoreOrm( IMapper mapper, DatabaseConfig config)
        {
            DatabaseConfig _config = config;
            _entityFrameworkDatabaseContext = new EntityFrameworkDatabaseContext(_config);
            _mapper = mapper;
        }

       
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling= TypeNameHandling.None,
            NullValueHandling = NullValueHandling.Ignore
        };

        private IDomainEvent TransformEvent(EventStoreDBSet eventSelected)
        {
            var o = JsonConvert.DeserializeObject<DomainEvent>(eventSelected.Data, _jsonSerializerSettings);
            var evt = o as IDomainEvent;

            return evt;
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

            
               
                await _entityFrameworkDatabaseContext.EventStore.AddRangeAsync(listOfEvents);
                 _entityFrameworkDatabaseContext.SaveChanges();
           
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
            
               
                await _entityFrameworkDatabaseContext.EventStore.AddRangeAsync(listOfEvents);
                await _entityFrameworkDatabaseContext.SaveChangesAsync();
                var ItemCategoryDto = _mapper.Map<ItemCategoryDto>(new ItemCategory(itemCategory.DomainEvents));
                
                return ItemCategoryDto;
           
        }

        public  async Task<IQueryable<ItemCategoryDto>> EventSourcingLoadAsync(IEntityId aggregateRootId)
        {
            if (aggregateRootId == null) throw new AggregateRootNotProvidedException("AggregateRootId cannot be null");



            var events =await _entityFrameworkDatabaseContext.EventStore.Where<EventStoreDBSet>(s => s.AggregateId == aggregateRootId.ToString()).ToListAsync();

            var itemevents = events.ConvertAll(x => new List<IDomainEvent>() { TransformEvent(x) }.AsReadOnly());

            var itemsresult = itemevents.ConvertAll(x => _mapper.Map<ItemCategoryDto>(new ItemCategory(x)));
            return itemsresult.AsQueryable();
        }

        public async Task<IQueryable<ItemCategoryDto>> EventSourcingLoadAllAsync()
        {
            var events = await _entityFrameworkDatabaseContext.EventStore.ToListAsync(); 
            var itemevents = events.ConvertAll(x => new List<IDomainEvent>() { TransformEvent(x) }.AsReadOnly());

            var itemsresult = itemevents.ConvertAll(x => _mapper.Map<ItemCategoryDto>(new ItemCategory(x)));
            return itemsresult.AsQueryable();
        }

        public async Task NonEventSourcingSaveAsync(ItemCategoryDto ItemCategoryDto)
        {
            if (ItemCategoryDto == null) return;
            
                _entityFrameworkDatabaseContext.ItemCategoryStoreDBSet.Add(new ItemCategoryStoreDBSet { Id = Guid.Parse(ItemCategoryDto.Id), CategoryName = ItemCategoryDto.CategoryName, Description = ItemCategoryDto.Description, Quantity = ItemCategoryDto.Quantity });
                await _entityFrameworkDatabaseContext.SaveChangesAsync();
           
        }

        public async Task<ItemCategoryDto> NonEventSourcingSaveWithResultAsync(ItemCategoryDto ItemCategoryDto)
        {
            if (ItemCategoryDto == null) return null;
            
                _entityFrameworkDatabaseContext.ItemCategoryStoreDBSet.Add(new ItemCategoryStoreDBSet { Id = Guid.Parse(ItemCategoryDto.Id), CategoryName = ItemCategoryDto.CategoryName, Description = ItemCategoryDto.Description, Quantity = ItemCategoryDto.Quantity });
                await _entityFrameworkDatabaseContext.SaveChangesAsync();
                return ItemCategoryDto;
           
        }

        public async Task<IQueryable<ItemCategoryDto>> NonEventSourcingLoadAsync(IEntityId aggregateRootId)
        {
            if (aggregateRootId == null) throw new IdException("Id cannot be null");

            var items =await _entityFrameworkDatabaseContext.ItemCategoryStoreDBSet.Where<ItemCategoryStoreDBSet>(s => s.Id == Guid.Parse(aggregateRootId.ToString())).ToListAsync(); 

           

            var selectedItem = _mapper.Map<List<ItemCategoryStoreDBSet>, List<ItemCategoryDto>>(items);

            return selectedItem.AsQueryable();
        }

        public async Task<IQueryable<ItemCategoryDto>> NonEventSourcingLoadAllAsync()
        {
            var items =await _entityFrameworkDatabaseContext.ItemCategoryStoreDBSet.ToListAsync();
            var itemsq = _mapper.Map<List<ItemCategoryStoreDBSet>, List<ItemCategoryDto>>(items);

            return itemsq.AsQueryable();
        }

        public async Task<ItemCategory> EventSourcingLoadEventAsync(IEntityId aggregateRootId)
        {
            if (aggregateRootId == null) throw new AggregateRootNotProvidedException("AggregateRootId cannot be null");


            
            var events =  await _entityFrameworkDatabaseContext.EventStore.Where<EventStoreDBSet>(s => s.AggregateId == aggregateRootId.ToString()).ToListAsync();
            var domainEvents = events.Select(TransformEvent).Where(x => x != null).ToList().AsReadOnly();
            var itemCategory = new ItemCategory(domainEvents);
            return  itemCategory;
        }

        public async Task<ItemCategoryDto> NonEventSourcingSaveUpdate(string id, string categoryName, string description, int quantity)
        {
           
                var itemcategory = _entityFrameworkDatabaseContext.ItemCategoryStoreDBSet.Where<ItemCategoryStoreDBSet>(item => item.Id == Guid.Parse(id)).FirstOrDefault();
                itemcategory.CategoryName = categoryName;
                itemcategory.Description = description;
                itemcategory.Quantity = quantity;
                _entityFrameworkDatabaseContext.Update(itemcategory);
                await _entityFrameworkDatabaseContext.SaveChangesAsync();
                return new ItemCategoryDto {Id= id, CategoryName=categoryName, Description=description, Quantity=quantity };

           
          
        }

        public async Task NonEventSourcingRemove(string id)
        {
           
                var itemcategory = _entityFrameworkDatabaseContext.ItemCategoryStoreDBSet.Where<ItemCategoryStoreDBSet>(item => item.Id == Guid.Parse(id)).FirstOrDefault();
                _entityFrameworkDatabaseContext.Remove(itemcategory);
                await _entityFrameworkDatabaseContext.SaveChangesAsync();
            
        }

       
    }
}
