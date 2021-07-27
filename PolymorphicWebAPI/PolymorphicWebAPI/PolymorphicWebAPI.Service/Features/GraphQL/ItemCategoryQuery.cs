
using App.Metrics;
using GraphQL;
using GraphQL.Types;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Metrics;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PolymorphicWebAPI.Service.Features.GraphQL
{
    public class ItemCategoryQuery : ObjectGraphType
    {
       
        public ItemCategoryQuery(ILogger<Object> logger, IMediator mediator, IMetrics metrics)
        {
            ILogger<Object> _logger = logger;
            IMediator _mediator = mediator;
            IMetrics _metrics = metrics;

            try
            {
                FieldAsync<ListGraphType<ItemCategoryGraphQLType>, IQueryable<ItemCategoryDto>>(
                   "allItemcategory",
                   "Returns all Item Categories.",
                  resolve: async context =>
                  {

                      var dtoObject = await _mediator.Send(new GetAllCategoryRequest());

                      return dtoObject;  
                    
                  });
            }
            catch (Exception exc)
            {
                _logger.LogError("Exception Occured :"+ exc.ToString());
                _metrics.Measure.Counter.Increment(MetricsRegistry.FailedRetrievedGraphQLItemCategory);

            }

            try
            {
                FieldAsync<ListGraphType<ItemCategoryGraphQLType>, IQueryable<ItemCategoryDto>>(
                "itemcategory",
               "Returns an Item Category.",
               new QueryArguments(
                   new QueryArgument<NonNullGraphType<IdGraphType>>
                   {
                       Name = "Id",
                       Description = "The unique guid of the item category."
                   }),
              resolve: async context => {
                var dtoObject= await _mediator.Send(new GetCategoryByIdRequest { Id = context.GetArgument("id", string.Empty) });
                  _metrics.Measure.Counter.Increment(MetricsRegistry.RetrievedGraphQLSingleItemCategory);
                  return dtoObject;

              });
               

            }
            catch (Exception exc)
            {
                _logger.LogError("Exception Occured :" + exc.ToString());
                _metrics.Measure.Counter.Increment(MetricsRegistry.FailedRetrievedGraphQLItemCategory);
                
            }

        }
    }
}
