using System;
using System.Net;
using System.Threading.Tasks;
using ComPact.Builders.V2;
using ComPact.Models;
using Xunit;

namespace Client.PactTests
{
    public class ApiTests : IClassFixture<ApiConsumerFixture>
    {
        private readonly PactBuilder _pactBuilder;
        private readonly ApiClient _apiClient;

        public ApiTests(ApiConsumerFixture fixture)
        {
            _apiClient = fixture._apiClient;

            _pactBuilder = fixture.PactBuilder;
        }

        [Fact]
        public async Task get_api_data()
        {
            _pactBuilder.SetUp(Pact.Interaction.Given(
                    $"An request for data")
                .UponReceiving("A get request for data")
                .With(Pact.Request.WithMethod(Method.GET)
                    .WithPath("/data")
                    .WithQuery("id=123"))
                .WillRespondWith(Pact.Response
                    .WithStatus((int) HttpStatusCode.OK)
                    .WithBody(Pact.JsonContent.With(
                        Some.Element.Named("Id").WithTheExactValue("123"),
                        Some.Element.Named("SomeInt").WithTheExactValue(1)))));

            await _apiClient.Get("123");
        }

        [Fact]
        public async Task get_api_data_not_found()
        {
            _pactBuilder.SetUp(Pact.Interaction.Given(
                    $"An request for data not found")
                .UponReceiving("A get request for data")
                .With(Pact.Request.WithMethod(Method.GET)
                    .WithPath("/data")
                    .WithQuery("id=not_found_id"))
                .WillRespondWith(Pact.Response
                    .WithStatus((int)HttpStatusCode.NotFound)));

            try
            {
                await _apiClient.Get("not_found_id");
            }
            catch (Exception e)
            {
            }
        }
    }
}
