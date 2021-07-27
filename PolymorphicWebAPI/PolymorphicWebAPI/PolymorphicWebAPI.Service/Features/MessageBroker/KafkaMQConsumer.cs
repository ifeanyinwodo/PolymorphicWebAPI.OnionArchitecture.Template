using Confluent.Kafka;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.MessageBroker
{
    public class KafkaMQConsumer : IMQConsumer
    {
        private readonly IMediator _mediator;
        private readonly MessageQueueOptions _messageQueueOptions;
        public KafkaMQConsumer(IMediator mediator, MessageQueueOptions messageQueueOptions)
        {
            _mediator = mediator;
            _messageQueueOptions = messageQueueOptions;
        }
        public async Task Consumer()
        {

            
           
            var config = new ConsumerConfig
            {
                
                GroupId = _messageQueueOptions.GroupIdKafaOnly,
                BootstrapServers = _messageQueueOptions.Host + ":" + _messageQueueOptions.Port,  
                AutoOffsetReset = AutoOffsetReset.Earliest,
               

            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
               
                 CancellationTokenSource cts = new CancellationTokenSource();

                  consumer.Subscribe(_messageQueueOptions.EndPointOrTopic);

                    
                            var consumerObject = consumer.Consume(cts.Token);
                            if (consumerObject.Message.Value != null)
                            {
                                ItemCategoryDto catalogItem = JsonConvert.DeserializeObject<ItemCategoryDto>(consumerObject.Message.Value);
                                
                                await _mediator.Send(new CreateCategoryRequest { Id = catalogItem.Id, CategoryName = catalogItem.CategoryName, Description = catalogItem.Description, Quantity = catalogItem.Quantity });
                            }
                  

            }

        }
    }
}
