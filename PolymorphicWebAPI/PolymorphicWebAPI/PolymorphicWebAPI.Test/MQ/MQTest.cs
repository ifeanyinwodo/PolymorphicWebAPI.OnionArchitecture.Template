using App.Metrics;
using AutoMapper;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Persistence.Repositories;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Commands;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using PolymorphicWebAPI.Service.Features.MessageBroker;
using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PolymorphicWebAPI.Test.MQ
{
    public class MQTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IStoreOrmRepository> _storeORMRepository;
        private readonly Mock<IMetrics> _metrics;
        private readonly Mock<EventAndNonEventStoreOrmFactory> _enStoreORMFactory;
        private readonly Mock<DatabaseConfig> _config;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<MQProducerFactory> _mQProducerFactory;
        private readonly MessageQueueOptions _messageQueueOptions;
        private readonly IConfiguration configuration;
        private readonly Mock<MQConsumerFactory> _mQConsumerFactory;
       
        public MQTest()
        {
            _mediator = new Mock<IMediator>();
            configuration = new TestConfiguration().Configuration;
            _messageQueueOptions = new MessageQueueOptions
            {
                Provider = configuration.GetValue<string>("MessageQueueOptions:Provider"),
                Host = configuration.GetValue<string>("MessageQueueOptions:Host"),
                Port = configuration.GetValue<string>("MessageQueueOptions:Port"),
                UserName = configuration.GetValue<string>("MessageQueueOptions:UserName"),
                Password = configuration.GetValue<string>("MessageQueueOptions:Password"),
                Type = configuration.GetValue<string>("MessageQueueOptions:Type"),
                EndPointOrTopic = configuration.GetValue<string>("MessageQueueOptions:EndPoint"),
                GroupIdKafaOnly = configuration.GetValue<string>("MessageQueueOptions:GroupIdKafaOnly"),
                Enable = configuration.GetValue<bool>("MessageQueueOptions:Enable")
            };
            
            _storeORMRepository = new Mock<IStoreOrmRepository>();
            _metrics = new Mock<IMetrics>();
            _config = new Mock<DatabaseConfig>();
            _mapper = new Mock<IMapper>();
            _enStoreORMFactory = new Mock<EventAndNonEventStoreOrmFactory>(_storeORMRepository.Object, _mapper.Object);
            _mQProducerFactory = new Mock<MQProducerFactory>(_messageQueueOptions);
            _mQConsumerFactory = new Mock<MQConsumerFactory>(_mediator.Object,_messageQueueOptions);





        }


        [EnableOrDisableTestBasedOnMQPublisherOption]
        public async Task Publisher_ExecuteAction()
        {

            CreateCategoryRequest createCategoryRequest = new CreateCategoryRequest
            {
                CategoryName = "Electric Iron",
                Description = "All types of electric Iron",
                Quantity = 45
            };

            await _mediator.Object.Send(createCategoryRequest);
           
            _mediator.Verify(x => x.Send(It.IsAny<CreateCategoryRequest>(), It.IsAny<CancellationToken>()));


          


        }

        [EnableOrDisableTestBasedOnMQConsumerOption]
        public async Task Consumer_ExecuteAction()
        {

            CreateCategoryRequest createCategoryRequest = new CreateCategoryRequest
            {
                CategoryName = "Electric Iron",
                Description = "All types of electric Iron",
                Quantity = 45
            };

            await _mediator.Object.Send(createCategoryRequest);
           
            _mediator.Verify(x => x.Send(It.IsAny<CreateCategoryRequest>(), It.IsAny<CancellationToken>()));





        }
    }
}
