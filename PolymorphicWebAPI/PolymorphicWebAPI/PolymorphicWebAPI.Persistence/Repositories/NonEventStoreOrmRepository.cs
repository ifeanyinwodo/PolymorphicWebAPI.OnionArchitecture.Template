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
    public class NonEventStoreOrmRepository : IEventAndNoneEventStoreOrmRepository
    {
        private readonly IStoreOrmRepository _storeORMRepository;
        private readonly IMapper _mapper;
        public NonEventStoreOrmRepository(IStoreOrmRepository storeORMRepository, IMapper mapper)
        {
            _storeORMRepository = storeORMRepository;
            _mapper = mapper;
        }
       
        public async Task<IQueryable<ItemCategoryDto>> GetItemCategory(string id)
        {

            var itemCategoryEvents = await _storeORMRepository.NonEventSourcingLoadAsync(new ItemCategoryId(id)); 

            return itemCategoryEvents;


        }
        public async Task<ItemCategoryDto> CreateItemCategory(string categoryName, string description, int quantity)
        {

            var itemCategory = ItemCategory.CreateNewItemCategory(categoryName, description, quantity);

            return await SaveItemCategoryAsync(itemCategory);
        }


        public async Task<ItemCategoryDto> SaveItemCategoryAsync(ItemCategory itemCategory)
        {
            var itemCategoryResult = _mapper.Map<ItemCategoryDto>(itemCategory);
            await _storeORMRepository.NonEventSourcingSaveAsync(itemCategoryResult);
           
            return itemCategoryResult;


        }


        public async Task<IQueryable<ItemCategoryDto>> GetAllItemCategory()
        {


            var itemCategoryEvents = await _storeORMRepository.NonEventSourcingLoadAllAsync();

            return itemCategoryEvents;


        }

        public async Task<ItemCategoryDto> UpdateItemCategoryAsync(ItemCategoryDto ItemCategoryDto)
        {
            var itemCategoryEvents = await _storeORMRepository.NonEventSourcingSaveUpdate(ItemCategoryDto);

            return itemCategoryEvents;
        }

       

        public Task RemoveCategoryAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
