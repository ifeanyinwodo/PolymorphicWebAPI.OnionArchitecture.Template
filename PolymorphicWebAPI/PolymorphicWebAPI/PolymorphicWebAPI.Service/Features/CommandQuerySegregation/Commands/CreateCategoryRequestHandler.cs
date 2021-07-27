using PolymorphicWebAPI.Persistence.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using PolymorphicWebAPI.Domain.DTO;
using App.Metrics;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Metrics;
using PolymorphicWebAPI.Domain.Entities;
using AutoMapper;
using PolymorphicWebAPI.Service.Features.MessageBroker;
using System.IO;
using PolymorphicWebAPI.Domain.Types;

namespace PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Commands
{
    public class CreateCategoryRequestHandler : IRequestHandler<CreateCategoryRequest, ItemCategoryDto>
    {
       
      
        private readonly IMetrics _metrics;
        private readonly EventAndNonEventStoreOrmFactory _enStoreORMFactory;
        private readonly DatabaseConfig _config;
        private readonly MQProducerFactory _mQProducerFactory;
        private readonly MessageQueueOptions _messageQueueOptions;
        public CreateCategoryRequestHandler(IStoreOrmRepository storeORMRepository, IMetrics metrics, DatabaseConfig config, IMapper mapper, MessageQueueOptions messageQueueOptions)
        {
             _messageQueueOptions = messageQueueOptions;
            IStoreOrmRepository _storeORMRepository = storeORMRepository;
            _metrics = metrics;
            _config = config;
            IMapper _mapper = mapper;
            _enStoreORMFactory = new EventAndNonEventStoreOrmFactory(_storeORMRepository, _mapper);
            _mQProducerFactory = new MQProducerFactory(_messageQueueOptions);
        }



        public async Task<ItemCategoryDto> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            
            var itemCategory = await _enStoreORMFactory.Create(_config.StoreType).CreateItemCategory(request.CategoryName, request.Description, request.Quantity);
           
            if (_messageQueueOptions.Enable && MessageQueueTypes.Producer.ToLower() == _messageQueueOptions.Type.ToLower()) await _mQProducerFactory.Create(_messageQueueOptions.Provider).Producer(itemCategory);
                
            _metrics.Measure.Counter.Increment(MetricsRegistry.CreatedMediatorItemCategory);
            return  itemCategory;
        }

      
    }
}
