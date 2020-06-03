using ComPact.Models;
using Moq;

namespace DataApi.PactTests
{
    public class DataControllerHandler
    {
        private readonly Mock<IDetermineIfIdsAreValid> _idService;

        public DataControllerHandler(Mock<IDetermineIfIdsAreValid> idService)
        {
            // Take in controller dependencies
            _idService = idService;
        }

        public void Handle(ProviderState providerState)
        {
            // setup responses of the dependencies based on state

            if (providerState.Name == "An request for data")
            {
                _idService.Setup(x => x.IsValidId(It.IsAny<string>())).Returns(true);
            }

            if (providerState.Name == "An request for data not found")
            {
                _idService.Setup(x => x.IsValidId(It.IsAny<string>())).Returns(false);
            }
        }
    }
}
