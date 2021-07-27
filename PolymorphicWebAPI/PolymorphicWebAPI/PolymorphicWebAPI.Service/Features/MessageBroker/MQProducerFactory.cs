using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Domain.Types;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.MessageBroker
{
    public class MQProducerFactory
    {
        private readonly Dictionary<string, IMQProducer> messageQueue = new Dictionary<string, IMQProducer>();
       
        public MQProducerFactory(MessageQueueOptions messageQueueOptions)
        {

            MessageQueueOptions _messageQueueOptions = messageQueueOptions;
            messageQueue.Add(MessageQueueProvider.Kafka, new KafkaMQProducer(_messageQueueOptions));
            messageQueue.Add(MessageQueueProvider.RabbitMQ, new RabbitMQProducer(_messageQueueOptions));
        }

        public IMQProducer Create(string messageQueueType)
        {
            
            return messageQueue[messageQueueType];
        }
    }
}
