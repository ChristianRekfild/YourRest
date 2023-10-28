using YourRest.Producer.Infrastructure.Keycloak.Http; 
namespace YourRest.WebApi.Tests.Fixtures
{
    public class TestHttpClientFactory : ICustomHttpClientFactory
    {
        public HttpClient CreateClient()
        {
            return new HttpClient();
        }
    }

}