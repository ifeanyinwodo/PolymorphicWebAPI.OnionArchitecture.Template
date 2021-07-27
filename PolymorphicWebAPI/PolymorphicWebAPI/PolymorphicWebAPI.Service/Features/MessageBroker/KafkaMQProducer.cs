using Confluent.Kafka;
using Confluent.Kafka.Admin;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.MessageBroker
{
    public class KafkaMQProducer : IMQProducer
    {

        private readonly MessageQueueOptions _messageQueueOptions;
        public KafkaMQProducer(MessageQueueOptions messageQueueOptions)
        {
           
            _messageQueueOptions = messageQueueOptions;
        }
        public async Task Producer(ItemCategoryDto ItemCategoryDto)
        {
            using(var adminClient= new AdminClientBuilder(new AdminClientConfig
            {
                BootstrapServers= _messageQueueOptions.Host + ":" + _messageQueueOptions.Port,
                
             }).Build())
            {
                
                var topic_metadata = adminClient.GetMetadata(new TimeSpan(100000)).Topics.Exists(p => p.Topic == _messageQueueOptions.EndPointOrTopic); 
                if (!topic_metadata)
                {
                        await adminClient.CreateTopicsAsync(new TopicSpecification[]
                         {
                        new TopicSpecification{Name=_messageQueueOptions.EndPointOrTopic,ReplicationFactor=1, NumPartitions=1}

                         });
                    }

            }
            var producerConfig = new ProducerConfig();
            producerConfig.BootstrapServers = _messageQueueOptions.Host+":"+ _messageQueueOptions.Port;

            string serializeItem = JsonConvert.SerializeObject(ItemCategoryDto);
            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                await producer.ProduceAsync(_messageQueueOptions.EndPointOrTopic, new Message<Null, string> { Value = serializeItem });

                producer.Flush(TimeSpan.FromSeconds(10));

            }

        }
    }
}
