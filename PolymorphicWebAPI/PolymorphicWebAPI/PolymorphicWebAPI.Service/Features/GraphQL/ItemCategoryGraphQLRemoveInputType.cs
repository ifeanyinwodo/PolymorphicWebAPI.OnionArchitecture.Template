using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.GraphQL
{
    public class ItemCategoryGraphQLRemoveInputType : InputObjectGraphType
    {
        public ItemCategoryGraphQLRemoveInputType()
        {
            Field<StringGraphType>("Id");
            
        }
    }
}
