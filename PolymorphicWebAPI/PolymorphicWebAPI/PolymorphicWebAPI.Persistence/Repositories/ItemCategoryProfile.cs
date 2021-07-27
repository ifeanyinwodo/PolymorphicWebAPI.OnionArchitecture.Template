using AutoMapper;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;


namespace PolymorphicWebAPI.Persistence.Repositories
{
    public class ItemCategoryProfile: Profile
    {
        public ItemCategoryProfile()
        {
            CreateMap<ItemCategory, ItemCategoryDto>();
            CreateMap<ItemCategoryStoreDBSet, ItemCategoryDto>();
            CreateMap<ItemCategoryDto, ItemCategoryStoreDBSet>();
            CreateMap<ItemCategoryDto, ItemCategory>().ForMember(i => i.Id, m => m.MapFrom(dto =>  new ItemCategoryId(dto.Id)));


        }
    }
}
