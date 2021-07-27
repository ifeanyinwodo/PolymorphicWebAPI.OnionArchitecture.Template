using App.Metrics;
using AutoMapper;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Persistence.Repositories;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Metrics;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Commands
{
    public class UpdateCategoryRequestHandler : IRequestHandler<UpdateCategoryRequest, ItemCategoryDto>
    {
      
        private readonly IMetrics _metrics;
        private readonly EventAndNonEventStoreOrmFactory _enStoreORMFactory;
        private readonly DatabaseConfig _config;
       

        public UpdateCategoryRequestHandler(IStoreOrmRepository storeORMRepository, IMetrics metrics, DatabaseConfig config, IMapper mapper)
        {

            IStoreOrmRepository _storeORMRepository = storeORMRepository;
            _metrics = metrics;
            _config = config;
            IMapper _mapper = mapper;
            _enStoreORMFactory = new EventAndNonEventStoreOrmFactory(_storeORMRepository, _mapper);
        }
        public async Task<ItemCategoryDto> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var itemCategory = await _enStoreORMFactory.Create(_config.StoreType).UpdateItemCategoryAsync( new ItemCategoryDto { Id = request.Id, CategoryName = request.CategoryName, Description = request.Description, Quantity = request.Quantity });

            _metrics.Measure.Counter.Increment(MetricsRegistry.CreatedMediatorItemCategory);
            return itemCategory;
        }
    }
}
