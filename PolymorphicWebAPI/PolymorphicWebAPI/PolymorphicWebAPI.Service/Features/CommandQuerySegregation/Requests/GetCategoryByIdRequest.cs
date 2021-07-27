using PolymorphicWebAPI.Domain.DTO;
using MediatR;
using System.Linq;

namespace PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests
{
   public class GetCategoryByIdRequest: IRequest<IQueryable<ItemCategoryDto>>
    {
       public string Id { get; set; }

    }
}
