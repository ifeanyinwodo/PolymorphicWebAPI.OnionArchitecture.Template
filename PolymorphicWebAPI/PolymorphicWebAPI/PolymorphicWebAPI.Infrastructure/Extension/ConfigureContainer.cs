using GraphQL.Server;
using PolymorphicWebAPI.Infrastructure.Middleware;
using PolymorphicWebAPI.Service.Features.GraphQL;
using Microsoft.AspNetCore.Builder;


namespace PolymorphicWebAPI.Infrastructure.Extension
{
    public static class ConfigureContainer
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }

        public static void ConfigureGraphQL(this IApplicationBuilder app)
        {
            app.UseGraphQLWebSockets<ItemCategorySchema>("/graphql");
            app.UseGraphQL<ItemCategorySchema>("/graphql");
        }
        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            
            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "PolymorphicWebAPI");
                setupAction.RoutePrefix = "OpenAPI";
            });
        }

        


    
    }
}
