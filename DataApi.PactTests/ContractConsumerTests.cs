using ComPact.Models;
using Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace DataApi.PactTests
{
    public class ConsumerContractTests
    {
        private readonly ContractVerifier<Startup> _contractVerifier;

        public ConsumerContractTests(ITestOutputHelper testOutput)
        {
            const int servicePort = 9494;

            _contractVerifier = new ContractVerifier<Startup>(testOutput, servicePort);
        }

        [Fact]
        public void VerifyInteractionWithClient()
        {
            var idServiceMock = new Mock<IDetermineIfIdsAreValid>();

            void ConfigureDi(IServiceCollection services)
            {
                services.AddSingleton(s => idServiceMock.Object);
            }

            void ConfigureState(ProviderState providerState)
            {
                if (providerState.Name == "An request for data")
                {
                    idServiceMock.Setup(x => x.IsValidId(It.IsAny<string>())).Returns(true);
                }

                if (providerState.Name == "An request for data not found")
                {
                    idServiceMock.Setup(x => x.IsValidId(It.IsAny<string>())).Returns(false);
                }
            }

            _contractVerifier.VerifyPact("data-api", "client", ConfigureState, ConfigureDi);
        }
    }
}
