using ComPact.Builders.V2;

namespace Helpers
{
    public static class ContractBuilder
    {
        public static PactBuilder BuildConsumerBuilder(string consumer, string provider,
            int mockServicePort = 4000, string pactDirectory = "../../../../pacts/") =>
            new PactBuilder(consumer, provider, $"http://0.0.0.0:{mockServicePort}", pactDir: pactDirectory);
    }
}