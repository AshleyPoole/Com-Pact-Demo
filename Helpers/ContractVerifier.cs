using System;
using ComPact.Models;
using ComPact.Verifier;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Helpers
{
    public class ContractVerifier<TStartup> where TStartup : class
    {
        private readonly int _servicePort;

        private readonly ITestOutputHelper _testOutput;

        public ContractVerifier(ITestOutputHelper testOutput, int servicePort = 4000)
        {
            _testOutput = testOutput;
            _servicePort = servicePort;
        }

        public void VerifyPact(
            string providerName,
            string consumerName,
            Action<ProviderState> providerState,
            Action<IServiceCollection> configureDi = null,
            IConfiguration configuration = null,
            string pactDirectory = "../../../../pacts"
        )
        {
            var baseUrl = $"http://localhost:{_servicePort}";

            var serviceBuilder = new HostBuilder()
                .ConfigureWebHostDefaults(c =>
                {
                    c.UseStartup<TStartup>();
                    c.UseEnvironment("Development");

                    if (configureDi != null)
                    {
                        c.ConfigureTestServices(configureDi);
                    }

                    c.UseUrls(baseUrl);

                    if (configuration != null)
                    {
                        c.UseConfiguration(configuration);
                    }

                    c.ConfigureLogging(l => l.AddXUnit(_testOutput));
                });

            using (serviceBuilder.StartAsync().GetAwaiter().GetResult())
            {
                var pactFilePath = $"{pactDirectory}/{consumerName}-{providerName}.json";
                var pactVerifier = new PactVerifier(new PactVerifierConfig { ProviderBaseUrl = baseUrl, ProviderStateHandler = providerState });

                pactVerifier.VerifyPactAsync(pactFilePath).GetAwaiter().GetResult();
            }
        }
    }
}
