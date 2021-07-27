using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Persistence.Repositories
{
    public interface IEventAndNoneEventStoreOrmRepository
    {
        
        Task<ItemCategoryDto> CreateItemCategory(string categoryName, string description, int quantity);

        Task<IQueryable<ItemCategoryDto>> GetItemCategory(string id);

        Task<ItemCategoryDto> UpdateItemCategoryAsync(ItemCategoryDto ItemCategoryDto);
        Task RemoveCategoryAsync(string id);
        
        Task<IQueryable<ItemCategoryDto>> GetAllItemCategory();
    }
}
