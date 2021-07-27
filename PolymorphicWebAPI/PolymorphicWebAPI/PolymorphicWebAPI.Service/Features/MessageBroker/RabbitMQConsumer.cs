using GreenPipes;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using MassTransit;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.MessageBroker
{
    public class RabbitMQConsumer: IMQConsumer
    {
       
        private readonly IMediator _mediator;
        private readonly MessageQueueOptions _messageQueueOptions;
       
        public RabbitMQConsumer(IMediator mediator, MessageQueueOptions messageQueueOptions)
        {
            _mediator = mediator;
            _messageQueueOptions = messageQueueOptions;

        }

        public async Task Consumer()
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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    var ItemCategoryDto = JsonConvert.DeserializeObject<ItemCategoryDto>(message);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, false);
                    Thread.Sleep(1000);
                    await _mediator.Send(new CreateCategoryRequest
                    {
                        Id = ItemCategoryDto.Id,
                        CategoryName = ItemCategoryDto.CategoryName,
                        Description = ItemCategoryDto.Description,
                        Quantity = ItemCategoryDto.Quantity

                    });
                };

                await Task.Run(() =>channel.BasicConsume(_messageQueueOptions.EndPointOrTopic, false, consumer));
                Thread.Sleep(1000);
            }
            


        }
    }
}
