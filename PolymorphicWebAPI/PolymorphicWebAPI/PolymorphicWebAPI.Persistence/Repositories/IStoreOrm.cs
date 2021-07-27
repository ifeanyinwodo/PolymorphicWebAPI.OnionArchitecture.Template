using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tactical.DDD;

namespace PolymorphicWebAPI.Persistence.Repositories
{
    public interface IStoreOrm
    {
       
        Task EventSourcingSaveAsync(ItemCategory itemCategory);
         
        Task<ItemCategoryDto> EventSourcingSaveWithResultAsync(ItemCategory itemCategory);

        Task<IQueryable<ItemCategoryDto>> EventSourcingLoadAsync(IEntityId aggregateRootId);
        Task<ItemCategory> EventSourcingLoadEventAsync(IEntityId aggregateRootId);

        Task<IQueryable<ItemCategoryDto>> EventSourcingLoadAllAsync();


        Task NonEventSourcingSaveAsync(ItemCategoryDto ItemCategoryDto);
        Task<ItemCategoryDto> NonEventSourcingSaveWithResultAsync(ItemCategoryDto ItemCategoryDto);

        Task<IQueryable<ItemCategoryDto>> NonEventSourcingLoadAsync(IEntityId aggregateRootId);
        Task<IQueryable<ItemCategoryDto>> NonEventSourcingLoadAllAsync();

        Task<ItemCategoryDto> NonEventSourcingSaveUpdate(string id, string categoryName, string description, int quantity);
        Task NonEventSourcingRemove(string id);

    }
}
