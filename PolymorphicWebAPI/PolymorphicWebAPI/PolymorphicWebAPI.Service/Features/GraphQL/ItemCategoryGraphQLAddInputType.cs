using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.GraphQL
{
    public class ItemCategoryGraphQLAddInputType : InputObjectGraphType
    {
        public ItemCategoryGraphQLAddInputType()
        {
           
            Field<StringGraphType>("CategoryName");
            Field<StringGraphType>("Description");
            Field<IntGraphType>("Quantity");
        }
    }
}
