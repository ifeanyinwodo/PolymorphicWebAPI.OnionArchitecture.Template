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
   
    public class GetCategoryByIdRequestHandler : IRequestHandler<GetCategoryByIdRequest, IQueryable<ItemCategoryDto>>
    {
       
        private readonly IMetrics _metrics;
        private readonly EventAndNonEventStoreOrmFactory _enStoreORMFactory;
        private readonly DatabaseConfig _config;
        private readonly IDistributedCache _radisDistributedCache;
        private readonly CacheOptions _cacheOptions;
        public GetCategoryByIdRequestHandler( IMetrics metrics, IStoreOrmRepository storeORMRepository, DatabaseConfig config, IMapper mapper, IDistributedCache radisDistributedCache, CacheOptions cacheOptions)
        {
            _radisDistributedCache = radisDistributedCache;
            IStoreOrmRepository _storeORMRepository = storeORMRepository;
            _metrics = metrics;
            _cacheOptions = cacheOptions;
            _config = config;
            IMapper _mapper = mapper;
            _enStoreORMFactory = new EventAndNonEventStoreOrmFactory(_storeORMRepository, _mapper);
        }

      

        public async Task<IQueryable<ItemCategoryDto>> Handle(GetCategoryByIdRequest request, CancellationToken cancellationToken)
        {
            _metrics.Measure.Counter.Increment(MetricsRegistry.RetrievedMediatorSingleItemCategory);

            if (_cacheOptions.EnableAzureRadis)
            {

                
                string getAllItemCategory = _radisDistributedCache.GetString("GetAllItemCategory");
                if (getAllItemCategory == null)
                {
                    
                    
                    var enStore = await _enStoreORMFactory.Create(_config.StoreType).GetItemCategory(request.Id);

                    if (enStore != null)
                    {
                        var enStoreAll = await _enStoreORMFactory.Create(_config.StoreType).GetAllItemCategory();
                        var options = new DistributedCacheEntryOptions();
                        options.SetAbsoluteExpiration(DateTimeOffset.Now.AddMinutes(_cacheOptions.ExpirationTimeInMinutes));
                        await _radisDistributedCache.SetStringAsync("GetAllItemCategory", JsonConvert.SerializeObject(enStoreAll), options, cancellationToken);
                        
                    }
                    return enStore;
                }
                else
                {
                    
                    return JsonConvert.DeserializeObject<EnumerableQuery<ItemCategoryDto>>(getAllItemCategory).Where(c=>c.Id == request.Id);
                }
            }
            else
            {
               
                
                return await _enStoreORMFactory.Create(_config.StoreType).GetItemCategory(request.Id);

            }


        }
    }
}
