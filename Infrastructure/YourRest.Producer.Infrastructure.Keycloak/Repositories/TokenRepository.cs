using YourRest.Domain.Repositories;
using YourRest.Domain.Models;
using Newtonsoft.Json;
using YourRest.Producer.Infrastructure.Keycloak.Serialization;
using YourRest.Producer.Infrastructure.Keycloak.Http;

namespace YourRest.Producer.Infrastructure.Keycloak.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ICustomHttpClientFactory _httpClientFactory;
        
        private const string ClientId = "your_rest_app";
        private const string ClientSecret = "qBC5V3wc2AYKTcYN1CACo6REU9t1Inrf";
        private const string KeycloakUrl = "http://keycloak:8080/auth/realms/YourRest/protocol/openid-connect/token";

        public TokenRepository(ICustomHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Token> GetTokenAsync(string username, string password)
        {
            using var httpClient = _httpClientFactory.CreateClient();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            var response = await httpClient.PostAsync(KeycloakUrl, content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Token>(result);
        }
    }
}