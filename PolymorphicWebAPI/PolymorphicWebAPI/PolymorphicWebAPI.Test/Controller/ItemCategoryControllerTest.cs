
using PolymorphicWebAPI.Controllers;
using PolymorphicWebAPI.Domain.DTO;
using PolymorphicWebAPI.Service.Features.CommandQuerySegregation.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Net;

namespace PolymorphicWebAPI.Test.Controller
{
    public class ItemCategoryControllerTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ItemCategoryController _itemCategoryController ;
        private readonly Mock<ILogger<ItemCategoryController>>_logger;
        private readonly HttpClient client;

        public ItemCategoryControllerTest()
        {
            
            _logger = new Mock<ILogger<ItemCategoryController>>();
            client = new TestClientProvider().Client;
            _mediator = new Mock<IMediator>();
            _itemCategoryController = new ItemCategoryController(_logger.Object, _mediator.Object);
        }

        [Fact]
        public async Task Post_Unit_ExecuteAction_InsertGroup()
        {

            var result = await _itemCategoryController.Post(new CreateCategoryRequest { Id = null, CategoryName = "Microsoft Sufface", Description = "Microsoft Personal computers", Quantity = 20 });
            var groupCollections = Assert.IsType<OkObjectResult>(result);
            _mediator.Verify(x => x.Send(It.IsAny<CreateCategoryRequest>(), It.IsAny<CancellationToken>()));


        }

        [Fact]
        public async Task Get_Unit_ExecuteAction_ReturnAllGroups()
        {
            var result = await _itemCategoryController.Get();
            var groupCollections = Assert.IsType<OkObjectResult>(result);
            var groupCollectionsResult = Assert.IsType<EnumerableQuery<ItemCategoryDto>>(groupCollections.Value);

            _mediator.Verify(x => x.Send(It.IsAny<GetAllCategoryRequest>(),It.IsAny<CancellationToken>()));
          
            
        }

        [Theory]
        [InlineData("Get", "api/v1/ItemCategory")]
       
        public async Task Get_ExecuteAction_ReturnAllGroups(string method, string URL)
        {
            
            var request = new HttpRequestMessage(new HttpMethod(method), URL);
            var response = await client.SendAsync(request);
            
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
        }

        [Theory]
        [InlineData( "api/v1/ItemCategory")]

        public async Task Post_ExecuteAction_InsertGroup( string restUrl)
        {


            string query = Newtonsoft.Json.JsonConvert.SerializeObject(new ItemCategoryDto { Id = "", CategoryName = "Television", Description = "All Brand of Television", Quantity = 70 });
            var content = new StringContent(query, Encoding.UTF8, "application/json");


            var response = await client.PostAsync(restUrl, content);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }






    }
}
