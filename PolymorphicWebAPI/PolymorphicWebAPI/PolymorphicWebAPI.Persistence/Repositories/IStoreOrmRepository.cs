using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tactical.DDD;

namespace PolymorphicWebAPI.Persistence.Repositories
{
    public interface IStoreOrmRepository
    {
        Task<IQueryable<ItemCategoryDto>> EventSourcingLoadAllAsync();
        Task<IQueryable<ItemCategoryDto>> EventSourcingLoadAsync(IEntityId aggregateRootId);

        Task<ItemCategory> EventSourcingLoadEventAsync(IEntityId aggregateRootId);
        Task EventSourcingSaveAsync(ItemCategory itemCategory);
       
        Task<ItemCategoryDto> EventSourcingSaveWithResultAsync(ItemCategory itemCategory);
        Task<IQueryable<ItemCategoryDto>> NonEventSourcingLoadAllAsync();
        Task<IQueryable<ItemCategoryDto>> NonEventSourcingLoadAsync(IEntityId aggregateRootId);
        Task NonEventSourcingSaveAsync(ItemCategoryDto ItemCategoryDto);
       
        Task<ItemCategoryDto> NonEventSourcingSaveWithResultAsync(ItemCategoryDto ItemCategoryDto);

        Task<ItemCategoryDto> NonEventSourcingSaveUpdate(ItemCategoryDto ItemCategoryDto);
        Task NonEventSourcingRemove(string id);
    }
}