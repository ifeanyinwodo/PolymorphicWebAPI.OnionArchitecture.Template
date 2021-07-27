using AutoMapper;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Persistence.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tactical.DDD;

namespace PolymorphicWebAPI.Persistence.Repositories
{
    public class StoreOrmRepository : IStoreOrmRepository
    {

       
        
        private readonly DatabaseConfig _config;
        private readonly StoreOrmFactory _storeORMFactory;
        public StoreOrmRepository( IMapper mapper, DatabaseConfig config)
        {

            IMapper _mapper = mapper;
            _config = config;
            _storeORMFactory = new StoreOrmFactory(_mapper, _config);

        }



        public Task<IQueryable<ItemCategoryDto>> EventSourcingLoadAllAsync()
        {
            return _storeORMFactory.Create(_config.ORMType).EventSourcingLoadAllAsync();
        }

        public Task<IQueryable<ItemCategoryDto>> EventSourcingLoadAsync(IEntityId aggregateRootId)
        {
            return _storeORMFactory.Create(_config.ORMType).EventSourcingLoadAsync(aggregateRootId);
        }

        public Task<ItemCategory> EventSourcingLoadEventAsync(IEntityId aggregateRootId)
        {
            return _storeORMFactory.Create(_config.ORMType).EventSourcingLoadEventAsync(aggregateRootId);
        }

        public Task EventSourcingSaveAsync(ItemCategory itemCategory)
        {
            return _storeORMFactory.Create(_config.ORMType).EventSourcingSaveAsync(itemCategory);
        }

        public Task<ItemCategoryDto> EventSourcingSaveWithResultAsync(ItemCategory itemCategory)
        {
            return _storeORMFactory.Create(_config.ORMType).EventSourcingSaveWithResultAsync(itemCategory);
        }

        public Task<IQueryable<ItemCategoryDto>> NonEventSourcingLoadAllAsync()
        {
            return _storeORMFactory.Create(_config.ORMType).NonEventSourcingLoadAllAsync();
        }

        public Task<IQueryable<ItemCategoryDto>> NonEventSourcingLoadAsync(IEntityId aggregateRootId)
        {
            return _storeORMFactory.Create(_config.ORMType).NonEventSourcingLoadAsync(aggregateRootId);
        }

        public async Task NonEventSourcingRemove(string id)
        {
            await _storeORMFactory.Create(_config.ORMType).NonEventSourcingRemove(id);

        }

        public Task NonEventSourcingSaveAsync(ItemCategoryDto ItemCategoryDto)
        {
            return _storeORMFactory.Create(_config.ORMType).NonEventSourcingSaveAsync(ItemCategoryDto);
        }

        public Task<ItemCategoryDto> NonEventSourcingSaveUpdate(ItemCategoryDto ItemCategoryDto)
        {
            return _storeORMFactory.Create(_config.ORMType).NonEventSourcingSaveUpdate(ItemCategoryDto.Id, ItemCategoryDto.CategoryName, ItemCategoryDto.Description, ItemCategoryDto.Quantity);
        }

        public Task<ItemCategoryDto> NonEventSourcingSaveWithResultAsync(ItemCategoryDto ItemCategoryDto)
        {
            return _storeORMFactory.Create(_config.ORMType).NonEventSourcingSaveWithResultAsync(ItemCategoryDto);
        }

    }
}
