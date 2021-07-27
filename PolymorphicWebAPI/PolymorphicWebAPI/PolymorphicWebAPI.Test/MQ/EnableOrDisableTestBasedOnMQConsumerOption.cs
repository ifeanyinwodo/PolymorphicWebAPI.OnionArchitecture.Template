using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Domain.Types;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PolymorphicWebAPI.Test.MQ
{
    public sealed class EnableOrDisableTestBasedOnMQConsumerOption:FactAttribute
    {
        
        private readonly IConfiguration configuration;
        public EnableOrDisableTestBasedOnMQConsumerOption()
        {
            configuration = new TestConfiguration().Configuration;
            if (configuration.GetValue<bool>("MessageQueueOptions:Enable") != true && MessageQueueTypes.Producer.ToLower() != configuration.GetValue<string>("MessageQueueOptions:Type").ToLower())
            {
                Skip = "Ignore message queue publisher test when message queue option is not enabled";
            }
            else if (MessageQueueTypes.Consumer.ToLower() != configuration.GetValue<string>("MessageQueueOptions:Type").ToLower())
            {
                Skip = "Ignore message queue consumer test when message queue option is enabled and publisher is specified";
            }
        }
    }
}
