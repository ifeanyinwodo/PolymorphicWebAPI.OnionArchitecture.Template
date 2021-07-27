using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.MessageBroker
{
    public interface IMQConsumer
    {
        Task Consumer();
    }
}
