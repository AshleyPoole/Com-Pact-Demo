using System;
using System.Net.Http;
using ComPact.Builders.V2;

namespace Client.PactTests
{
    public class ApiConsumerFixture : IDisposable
    {
        public ApiClient _apiClient;

        public PactBuilder PactBuilder { get; }

        public ApiConsumerFixture()
        {
            const string mockServerBaseUrl = "http://localhost:5000";

            PactBuilder = new PactBuilder("client", "data-api", mockServerBaseUrl, pactDir: "../../../../pacts/");

            _apiClient = new ApiClient(new HttpClient { BaseAddress = new Uri(mockServerBaseUrl) });
        }

        public void Dispose() => PactBuilder.BuildAsync().GetAwaiter().GetResult();
    }
}