using AutoMapper;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Persistence.Repositories
{
    public class EventStoreOrmRepository : IEventAndNoneEventStoreOrmRepository
    {
        private readonly IStoreOrmRepository _storeORMRepository;
        private readonly IMapper _mapper;
        public EventStoreOrmRepository(IStoreOrmRepository storeORMRepository, IMapper mapper)
        {
            _storeORMRepository = storeORMRepository;
            _mapper = mapper;
        }
        public async Task<IQueryable<ItemCategoryDto>> GetItemCategory(string id)
        {
           
                var itemCategoryEvents = await _storeORMRepository.EventSourcingLoadAsync(new ItemCategoryId(id)); 

                return itemCategoryEvents;
            
            
        }
        public async Task<ItemCategoryDto> CreateItemCategory(string categoryName, string description, int quantity)
        {

            var itemCategory = ItemCategory.CreateNewItemCategory(categoryName, description, quantity);
            return await SaveItemCategoryAsync(itemCategory);
            
        }


        public async Task<ItemCategoryDto> SaveItemCategoryAsync(ItemCategory itemCategory)
        {
            
                 await _storeORMRepository.EventSourcingSaveAsync(itemCategory);
                 var itemCategoryResult= _mapper.Map<ItemCategoryDto>(itemCategory);
                return itemCategoryResult;
          

        }


        public async Task<IQueryable<ItemCategoryDto>> GetAllItemCategory()
        {
            

                var itemCategoryEvents = await _storeORMRepository.EventSourcingLoadAllAsync();

                return itemCategoryEvents;

           
        }

        public async Task<ItemCategoryDto> UpdateItemCategoryAsync(ItemCategoryDto ItemCategoryDto)
        {
            var itemCategoryEvents = await _storeORMRepository.EventSourcingLoadEventAsync(new ItemCategoryId(ItemCategoryDto.Id));
           
            ItemCategory itemCategory = itemCategoryEvents;
            itemCategory.UpdateItemCategory(ItemCategoryDto.Id, ItemCategoryDto.CategoryName, ItemCategoryDto.Description, ItemCategoryDto.Quantity);
            return await SaveItemCategoryAsync(itemCategory); 
            

        }

        public async Task RemoveCategoryAsync(string id)
        {
            
            var itemCategoryEvents = await _storeORMRepository.EventSourcingLoadEventAsync(new ItemCategoryId(id));
           
            ItemCategory itemCategory = itemCategoryEvents;
            itemCategory.RemoveItemCategory(itemCategoryEvents.Id.ToString(), itemCategory.CategoryName, itemCategory.Description, itemCategory.Quantity);
            await SaveItemCategoryAsync(itemCategory); 
        }
    }
}
