using App.Metrics;
using GraphQL.Types;
using MediatR;
using Microsoft.Extensions.Logging;
using System;


namespace PolymorphicWebAPI.Service.Features.GraphQL
{
   public class ItemCategorySchema :Schema
    {
       
       
        
        public ItemCategorySchema(IServiceProvider serviceProvider, ILogger<ItemCategorySchema> logger, IMediator mediator) : base(serviceProvider)
        {
            ILogger<ItemCategorySchema>  _logger = logger;
            IMetrics _metrics= new MetricsBuilder().Build();
            IMediator _mediator = mediator;
            Query = new ItemCategoryQuery(_logger, _mediator, _metrics);
            Mutation = new ItemCategoryMutation(_logger, _mediator, _metrics);
        }
    }
}
