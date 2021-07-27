using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using Microsoft.Extensions.DependencyInjection;
using System;
using App.Metrics;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Metrics;
using Microsoft.Extensions.Logging;
using System.Linq;
using PolymorphicWebAPI.Domain.DTO;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PolymorphicWebAPI.Controllers
{
    
    [Route("api/v{version:apiVersion}/ItemCategory")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ItemCategoryController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<ItemCategoryController> _logger;
        internal readonly IMetrics _metrics;

       
        public ItemCategoryController(ILogger<ItemCategoryController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            _metrics =  new MetricsBuilder().Build(); 
        }

        
        
        // GET: api/<ItemCategoryController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            try
            {

                _metrics.Measure.Counter.Increment(MetricsRegistry.RetrievedRestAllItemCategory);
                return Ok(await _mediator.Send(new GetAllCategoryRequest()));
             }
            catch (Exception exc)
            {
                _logger.LogError("Exception Occured :" + exc.ToString());
                _metrics.Measure.Counter.Increment(MetricsRegistry.FailedRetrievedRestItemCategory);
                return NotFound(exc.ToString());
             }
}

        // GET api/<ItemCategoryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
           

            try
            {

                _metrics.Measure.Counter.Increment(MetricsRegistry.RetrievedRestSingleItemCategory);
                return Ok(await _mediator.Send(new GetCategoryByIdRequest { Id = id }));
            }
            catch (Exception exc)
            {
                _logger.LogError("Exception Occured :" + exc.ToString());
                _metrics.Measure.Counter.Increment(MetricsRegistry.FailedRetrievedRestItemCategory);
                return NotFound();
            }
        }

        // POST api/<ItemCategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCategoryRequest createCategoryRequest )
        {
           
               
            try
            {
               
                _metrics.Measure.Counter.Increment(MetricsRegistry.CreatedRestItemCategory);
                return Ok(await _mediator.Send(createCategoryRequest));
            }
            catch (Exception exc)
            {
                _logger.LogError("Exception Occured :" + exc.ToString());
                _metrics.Measure.Counter.Increment(MetricsRegistry.FailedCreatedRestItemCategory);
                return NotFound();
            }

        }

        // PUT api/<ItemCategoryController>
        
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryRequest updateCategoryRequest)
        {

                return Ok(await _mediator.Send(updateCategoryRequest));
           
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove([FromBody] string id)
        {

            await _mediator.Send(new RemoveCategoryRequest { Id = id });
            return Ok();

        }


    }
}
