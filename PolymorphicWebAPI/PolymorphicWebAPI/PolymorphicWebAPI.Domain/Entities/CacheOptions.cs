using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Domain.Entities
{
   public class CacheOptions
    {
        public bool EnableAzureRadis { get; set; }

        public int ExpirationTimeInMinutes { get; set; }

        public string ConnectionString { get; set; }
    }
}
