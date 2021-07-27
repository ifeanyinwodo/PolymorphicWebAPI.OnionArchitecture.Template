using PolymorphicWebAPI.Domain.DTO;
using MediatR;


namespace PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests
{
   public class CreateCategoryRequest:IRequest<ItemCategoryDto>
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

       
    }
}
