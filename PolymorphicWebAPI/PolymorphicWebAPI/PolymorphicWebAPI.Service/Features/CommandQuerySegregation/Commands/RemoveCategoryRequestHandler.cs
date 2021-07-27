using App.Metrics;
using AutoMapper;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Persistence.Repositories;
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
    public class RemoveCategoryRequestHandler : IRequestHandler<RemoveCategoryRequest, ItemCategoryDto>
    {

       
        private readonly EventAndNonEventStoreOrmFactory _enStoreORMFactory;
        private readonly DatabaseConfig _config;
      

        public RemoveCategoryRequestHandler(IStoreOrmRepository storeORMRepository, IMetrics metrics, DatabaseConfig config, IMapper mapper)
        {

            IStoreOrmRepository _storeORMRepository = storeORMRepository;
            _config = config;
            IMapper _mapper = mapper;
            _enStoreORMFactory = new EventAndNonEventStoreOrmFactory(_storeORMRepository, _mapper);
        }

        public async Task<ItemCategoryDto> Handle(RemoveCategoryRequest request, CancellationToken cancellationToken)
        {
            await _enStoreORMFactory.Create(_config.StoreType).RemoveCategoryAsync(request.Id);
            return null;
        }
        
    }
}
