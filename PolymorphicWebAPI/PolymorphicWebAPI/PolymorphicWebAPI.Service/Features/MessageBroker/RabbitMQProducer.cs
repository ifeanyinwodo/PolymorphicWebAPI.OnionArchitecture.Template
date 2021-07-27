using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using MassTransit;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.MessageBroker
{
    public class RabbitMQProducer: IMQProducer
    {
        private readonly MessageQueueOptions _messageQueueOptions;
        public RabbitMQProducer(MessageQueueOptions messageQueueOptions)
        {

            _messageQueueOptions = messageQueueOptions;
        }
        public async Task Producer(ItemCategoryDto ItemCategoryDto)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _messageQueueOptions.Host,
                UserName = _messageQueueOptions.UserName,
                Password = _messageQueueOptions.Password,
                Port = int.Parse(_messageQueueOptions.Port)

            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _messageQueueOptions.EndPointOrTopic,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = JsonConvert.SerializeObject(ItemCategoryDto);
                var messageBytes = Encoding.UTF8.GetBytes(message);
                await Task.Run(() =>channel.BasicPublish(exchange: "",
                    routingKey: _messageQueueOptions.EndPointOrTopic,
                    basicProperties: null,
                    body: messageBytes));

                
            }

           
        }
    }
}
