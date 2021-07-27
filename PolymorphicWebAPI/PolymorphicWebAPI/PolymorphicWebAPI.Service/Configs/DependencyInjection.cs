using PolymorphicWebAPI.Persistence.Repositories;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PolymorphicWebAPI.Service.Configs
{
    public static class DependencyInjection
    {
        public static void AddServiceLayer(this IServiceCollection services)
        {

            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            services.AddGrpc();

        }



    }
}
