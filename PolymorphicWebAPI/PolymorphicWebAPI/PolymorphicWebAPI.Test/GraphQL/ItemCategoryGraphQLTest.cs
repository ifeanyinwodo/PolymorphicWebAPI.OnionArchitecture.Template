
using MediatR;
using Moq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PolymorphicWebAPI.Test.GraphQL
{

    public class ItemCategoryGraphQLTest
    {
        private readonly HttpClient client;
        private readonly Mock<IMediator> _mediator;
        public ItemCategoryGraphQLTest()
        {
            client = new TestClientProvider().Client;
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task Post_ExecuteAction_InsertGroup()
        {

            const string query = @"{
                ""query"": ""mutation{addItemCategory(createCategoryRequest: {categoryName:\""Radio\"",description:\""Winwof Radio Device\"",quantity:40}){id,categoryName,description,quantity}}""
            }";
            var content = new StringContent(query, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/graphql", content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task Get_ExecuteAction_ReturnAllGroups()
        {

            
            const string query = @"{
                ""query"": ""query{allItemcategory{id categoryName description quantity}}""
            }";
            var content = new StringContent(query, Encoding.UTF8, "application/json");

            
            var response = await client.PostAsync("/graphql", content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }




    }
}
