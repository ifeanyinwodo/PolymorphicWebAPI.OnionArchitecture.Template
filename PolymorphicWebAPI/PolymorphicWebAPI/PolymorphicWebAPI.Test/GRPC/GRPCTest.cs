
using System;
using System.Net.Http;
using Xunit;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System.Threading;
using Grpc.Core;
using PolymorphicWebAPI.Service.Features.GRPC;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Test.GRPC
{
    public class GRPCTest
    {
        private readonly HttpClient client;

        public GRPCTest()
        {
            client = new TestClientProvider().Client;
            
        }

   



        [Theory]
        [InlineData("https://localhost:5001")]
        public async Task Post_ExecuteAction_InsertGroupAsync(string serviceUrl)
        {
            GrpcChannelOptions options = new()
            {
                HttpClient = client
            };
          
            var channel = GrpcChannel.ForAddress(serviceUrl, options);

#pragma warning disable CS0436 // Type conflicts with imported type
            var itemCategoriesClient = new PolymorphicWebAPI.Service.Features.GRPC.ItemCategories.ItemCategoriesClient(channel);
#pragma warning restore CS0436 // Type conflicts with imported type

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10000));

#pragma warning disable CS0436 // Type conflicts with imported type
            var category =await itemCategoriesClient.PostAsync(new Category
#pragma warning restore CS0436 // Type conflicts with imported type
             {
                 Id="",
                 CategoryName="Rugs",
                 Description ="All Types of Rugs",
                 Quantity=30

            }, cancellationToken: cts.Token);
           
            Assert.NotNull(category.Id);
        }

        [Theory]
        [InlineData("https://localhost:5001")]
        public async Task Get_ExecuteAction_ReturnAllGroups(string serviceUrl)
        {
            GrpcChannelOptions options = new GrpcChannelOptions()
            {
                HttpClient = client
            };
           
            var channel = GrpcChannel.ForAddress(serviceUrl, options);

#pragma warning disable CS0436 // Type conflicts with imported type
            var itemCategoriesClient = new ItemCategories.ItemCategoriesClient(channel);
#pragma warning restore CS0436 // Type conflicts with imported type

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(100));
            var records = 0;
            using var streamCall = itemCategoriesClient.Get(new Empty(), cancellationToken: cts.Token);

#pragma warning disable CS0436 // Type conflicts with imported type
            await foreach (var itmGroup in streamCall.ResponseStream.ReadAllAsync<GetAllCategories>()) //cancellationToken: cts.Token
                {
                   
                    records++;
                    Thread.Sleep(100);
                }
#pragma warning restore CS0436 // Type conflicts with imported type     
            Assert.NotEqual(0, records);
        }
    }
}
