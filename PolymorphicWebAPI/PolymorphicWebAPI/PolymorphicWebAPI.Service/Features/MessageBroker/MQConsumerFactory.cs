using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Domain.Types;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.MessageBroker
{
    public class MQConsumerFactory
    {
        private readonly Dictionary<string, IMQConsumer> messageQueue = new Dictionary<string, IMQConsumer>();
       
       
        public MQConsumerFactory(IMediator mediator, MessageQueueOptions messageQueueOptions)
        {
            IMediator _mediator = mediator;
            MessageQueueOptions _messageQueueOptions = messageQueueOptions;
            messageQueue.Add(MessageQueueProvider.Kafka, new KafkaMQConsumer(_mediator, _messageQueueOptions));
            messageQueue.Add(MessageQueueProvider.RabbitMQ, new RabbitMQConsumer(_mediator, _messageQueueOptions));
        }

        public IMQConsumer Create(string messageQueueType)
        {
            return  messageQueue[messageQueueType];
        }
    }
}
