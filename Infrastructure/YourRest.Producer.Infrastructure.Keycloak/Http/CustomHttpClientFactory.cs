using System.Net.Http;

namespace YourRest.Producer.Infrastructure.Keycloak.Http
{
    public class CustomHttpClientFactory : ICustomHttpClientFactory
    {
        private readonly IHttpClientFactory _customHttpClientFactory;

        public CustomHttpClientFactory(IHttpClientFactory customHttpClientFactory)
        {
            _customHttpClientFactory = customHttpClientFactory;
        }

        public HttpClient CreateClient()
        {
            return _customHttpClientFactory.CreateClient();
        }
    }
}