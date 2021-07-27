using PolymorphicWebAPI.Domain.DTO;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests
{
    public record GetAllCategoryRequest() : IRequest<IQueryable<ItemCategoryDto>>;
   
}
