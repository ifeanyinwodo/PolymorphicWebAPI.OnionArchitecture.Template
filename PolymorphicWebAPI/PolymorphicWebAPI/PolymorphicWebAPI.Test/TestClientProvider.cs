using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Test
{
    public class TestClientProvider
    {
        public HttpClient Client { get; private set; }

        public TestClientProvider()
        {
            

            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Test")
                .UseStartup<Startup>()
                );
            Client = server.CreateClient();
        }

    }
}
