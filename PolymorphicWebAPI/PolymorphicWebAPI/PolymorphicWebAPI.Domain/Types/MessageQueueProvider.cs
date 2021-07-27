using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Domain.Types
{
    public static class MessageQueueProvider
    {
       
        public const string Kafka = "kafka";
        public const string RabbitMQ = "rabbitmq";
    }
}
