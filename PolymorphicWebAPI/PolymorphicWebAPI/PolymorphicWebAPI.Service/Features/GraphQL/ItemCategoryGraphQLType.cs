using GraphQL.Types;
using PolymorphicWebAPI.Domain.DTO;



namespace PolymorphicWebAPI.Service.Features.GraphQL
{
    public class ItemCategoryGraphQLType : ObjectGraphType<ItemCategoryDto>
    {
        public ItemCategoryGraphQLType()
        {
            Field(o => o.Id, type: typeof(StringGraphType));
            Field(o => o.CategoryName, type: typeof(StringGraphType));
            Field(o => o.Description, type: typeof(StringGraphType));
            Field(o => o.Quantity, type: typeof(IntGraphType));

        }
    }
}
