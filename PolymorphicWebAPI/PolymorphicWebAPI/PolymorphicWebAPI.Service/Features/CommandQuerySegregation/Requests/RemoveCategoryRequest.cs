using PolymorphicWebAPI.Domain.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests
{
    public class RemoveCategoryRequest : IRequest<ItemCategoryDto>
    {
        public string Id { get; set; }
    }
}
