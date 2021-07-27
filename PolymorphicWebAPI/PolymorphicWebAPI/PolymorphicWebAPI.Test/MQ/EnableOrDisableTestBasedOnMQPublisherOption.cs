using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Domain.Types;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace PolymorphicWebAPI.Test.MQ
{
    public sealed class EnableOrDisableTestBasedOnMQPublisherOption :FactAttribute
    {
        private readonly IConfiguration configuration;
        public EnableOrDisableTestBasedOnMQPublisherOption()
        {
            configuration = new TestConfiguration().Configuration;
 
            if (configuration.GetValue<bool>("MessageQueueOptions:Enable") != true && MessageQueueTypes.Producer.ToLower() != configuration.GetValue<string>("MessageQueueOptions:Type").ToLower())
            {
                Skip = "Ignore message queue publisher test when message queue option is not enabled";
            }
            else if (MessageQueueTypes.Producer.ToLower() != configuration.GetValue<string>("MessageQueueOptions:Type").ToLower())
            {
                Skip = "Ignore message queue publisher test when message queue option is enabled and consumer is specified";
            }
            

        }
    }
}
