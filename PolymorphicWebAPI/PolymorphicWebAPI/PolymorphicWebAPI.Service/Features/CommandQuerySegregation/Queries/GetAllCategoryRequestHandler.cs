using App.Metrics;
using AutoMapper;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Persistence.Repositories;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Metrics;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Queries
{
    public class GetAllCategoryRequestHandler : IRequestHandler<GetAllCategoryRequest, IQueryable<ItemCategoryDto>>
    {
       
        private readonly IMetrics _metrics;
        public readonly IStoreOrmRepository _storeORMRepository;
        private readonly EventAndNonEventStoreOrmFactory _enStoreORMFactory;
        private readonly DatabaseConfig _config;
        private readonly IDistributedCache _radisDistributedCache;
        private readonly CacheOptions _cacheOptions;
        
        public GetAllCategoryRequestHandler( IMetrics metrics, IStoreOrmRepository storeORMRepository, DatabaseConfig config, IMapper mapper, IDistributedCache radisDistributedCache, CacheOptions cacheOptions) 
        {
            _radisDistributedCache = radisDistributedCache;
           _storeORMRepository = storeORMRepository;
            _metrics = metrics;
            _config = config;
            _cacheOptions = cacheOptions;
            IMapper _mapper = mapper;
            _enStoreORMFactory = new EventAndNonEventStoreOrmFactory(_storeORMRepository, _mapper);
        }

        public async Task<IQueryable<ItemCategoryDto>> Handle(GetAllCategoryRequest request, CancellationToken cancellationToken)
        {
            _metrics.Measure.Counter.Increment(MetricsRegistry.RetrievedMediatorAllItemCategory);
            if (_cacheOptions.EnableAzureRadis)
            {
               
               
                string getAllItemCategory = _radisDistributedCache.GetString("GetAllItemCategory");
                if (getAllItemCategory == null)
                {
                   
                    var enStore = await _enStoreORMFactory.Create(_config.StoreType).GetAllItemCategory();

                    if (enStore != null)
                    {
                        var options = new DistributedCacheEntryOptions();
                        options.SetAbsoluteExpiration(DateTimeOffset.Now.AddMinutes(_cacheOptions.ExpirationTimeInMinutes));
                        await _radisDistributedCache.SetStringAsync("GetAllItemCategory", JsonConvert.SerializeObject(enStore), options, cancellationToken);
                       
                    }
                    return enStore;
                }
                else
                {
                    
                    return JsonConvert.DeserializeObject<EnumerableQuery<ItemCategoryDto>>(getAllItemCategory);
                }
            }
            else
            {
                
               
                var enStore = await _enStoreORMFactory.Create(_config.StoreType).GetAllItemCategory();
                return enStore;
            }





        }
    }
}
