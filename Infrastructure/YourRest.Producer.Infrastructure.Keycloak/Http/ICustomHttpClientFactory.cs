namespace YourRest.Producer.Infrastructure.Keycloak.Http
{
    public interface ICustomHttpClientFactory
    {
        HttpClient CreateClient();
    }
}