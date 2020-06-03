using System.Threading;
using System.Threading.Tasks;
using ComPact.Verifier;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace DataApi.PactTests
{
    public class ConsumerContractTests
    {
        private readonly string _url;

        public ConsumerContractTests(ITestOutputHelper testOutput)
        {
            _url = "http://localhost:9494";
        }

        [Fact]
        public async Task VerifyInteractionWithClient()
        {
            var idServiceMock = new Mock<IDetermineIfIdsAreValid>();
            var providerState = new DataControllerHandler(idServiceMock);

            var pactVerifier = new PactVerifier(new PactVerifierConfig { ProviderBaseUrl = _url, ProviderStateHandler = providerState.Handle });

            var cts = new CancellationTokenSource();

            var hostTask = new HostBuilder()
                .ConfigureWebHostDefaults(c =>
                {
                    c.UseStartup<Startup>();
                    c.UseEnvironment("Development");
                    c.ConfigureTestServices(services => { services.AddSingleton(s => idServiceMock.Object); });
                    c.UseUrls(_url);
                }).Build().RunAsync(cts.Token);

            const string pactDir = "../../../../pacts/";
            await pactVerifier.VerifyPactAsync(pactDir + "client-data-api.json");

            cts.Cancel();
            await hostTask;
        }
    }
}
