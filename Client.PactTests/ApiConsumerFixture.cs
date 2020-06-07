using System;
using System.Net.Http;
using ComPact.Builders.V2;
using Helpers;

namespace Client.PactTests
{
    public class ApiConsumerFixture : IDisposable
    {
        public ApiClient _apiClient;

        public PactBuilder _pactBuilder { get; }

        public ApiConsumerFixture()
        {
            const int mockServerPort = 5000;

            _pactBuilder = ContractBuilder.BuildConsumerBuilder("client", "data-api", mockServerPort);
            _apiClient = new ApiClient(new HttpClient { BaseAddress = new Uri($"http://localhost:{mockServerPort}") });
        }

        public void Dispose() => _pactBuilder.BuildAsync().GetAwaiter().GetResult();
    }
}