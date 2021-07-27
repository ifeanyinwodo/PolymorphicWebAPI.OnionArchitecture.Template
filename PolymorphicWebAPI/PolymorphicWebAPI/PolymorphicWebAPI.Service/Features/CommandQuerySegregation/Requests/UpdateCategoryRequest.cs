using PolymorphicWebAPI.Domain.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests
{
    public class UpdateCategoryRequest : IRequest<ItemCategoryDto>
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }


    }
}
