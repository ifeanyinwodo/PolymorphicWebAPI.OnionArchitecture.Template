using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Domain.Entities
{
    public class MessageQueueOptions
    {
        public string Provider { get; set; }
       
        public string Host { get; set; }

        public string Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Type { get; set; }

        public string EndPointOrTopic { get; set; }

        public string GroupIdKafaOnly { get; set; }

        public bool Enable { get; set; }


    }
}
