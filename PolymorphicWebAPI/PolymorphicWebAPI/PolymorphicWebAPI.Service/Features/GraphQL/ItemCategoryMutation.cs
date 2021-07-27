using App.Metrics;
using GraphQL;
using GraphQL.Types;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Metrics;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using MediatR;
using Microsoft.Extensions.Logging;
using System;

namespace PolymorphicWebAPI.Service.Features.GraphQL
{
    public class ItemCategoryMutation : ObjectGraphType<object>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<Object> _logger;
        private readonly IMetrics _metrics;
        public ItemCategoryMutation(ILogger<Object> logger, IMediator mediator, IMetrics metrics)
        {
            _logger = logger;
            _mediator = mediator;
            _metrics = metrics;

            try
            {
                FieldAsync<ItemCategoryGraphQLType, ItemCategoryDto>(
               "AddItemCategory",
               "Add Item Category.",
               new QueryArguments(
                  
                   new QueryArgument<ItemCategoryGraphQLAddInputType>
                   {
                       Name = "createCategoryRequest",
                       Description = "Item Category ."
                   }),
               resolve: context =>
               {
                  
                   var createCategoryRequest = context.GetArgument<CreateCategoryRequest>("createCategoryRequest");
                   var dtoObject = _mediator.Send(createCategoryRequest);
                   _metrics.Measure.Counter.Increment(MetricsRegistry.CreatedGraphQLItemCategory);

                   return dtoObject;
                  
               });

            FieldAsync<ItemCategoryGraphQLType, ItemCategoryDto>(
               "UpdateItemCategory",
               "Update Item Category.",
               new QueryArguments(

                   new QueryArgument<ItemCategoryGraphQLUpdateInputType>
                   {
                       Name = "updateCategoryRequest",
                       Description = "Item Category ."
                   }),
               resolve: context =>
               {

                   var createCategoryRequest = context.GetArgument<UpdateCategoryRequest>("updateCategoryReques");
                   var dtoObject = _mediator.Send(createCategoryRequest);
                   _metrics.Measure.Counter.Increment(MetricsRegistry.CreatedGraphQLItemCategory);

                   return dtoObject;

               });
            FieldAsync<ItemCategoryGraphQLType, ItemCategoryDto>(
               "RemoveItemCategory",
               "Remove Item Category.",
               new QueryArguments(

                   new QueryArgument<ItemCategoryGraphQLRemoveInputType>
                   {
                       Name = "removeCategoryRequest",
                       Description = "Item Category ."
                   }),
               resolve: context =>
               {

                   var createCategoryRequest = context.GetArgument<RemoveCategoryRequest>("removeCategoryRequest");
                   var dtoObject = _mediator.Send(createCategoryRequest);
                  

                   return dtoObject;

               });
            }
            catch (Exception exc)
            {
                _logger.LogError("Exception Occured :" + exc.ToString());
                _metrics.Measure.Counter.Increment(MetricsRegistry.FailedCreatedGraphQLItemCategory);

            }
        }
    }
}
