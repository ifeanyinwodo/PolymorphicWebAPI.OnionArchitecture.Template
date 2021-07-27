using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using PolymorphicWebAPI.Service.Features.Worker;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace PolymorphicWebAPI
{
   
    public class Program
    {

        
        public static void Main(string[] args)
        {
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .UseMetrics(options => {
                 options.EndpointOptions = endpointsOptions =>
                 {
                     endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                     endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                     endpointsOptions.EnvironmentInfoEndpointEnabled = false;
                 };
                 })
                 .UseMetricsWebTracking()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();


                });
    }
}
