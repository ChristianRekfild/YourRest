using Newtonsoft.Json;
using System.Text;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services.Abstractions;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly HttpClient httpClient;
        private readonly string WebApiUrl;

        public AuthorizationService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClient = httpClientFactory.CreateClient();
            WebApiUrl = configuration.GetSection("webApiUrl").Value;
        }
        public async Task<HttpResponseMessage> LoginAsync(AuthorizationModel login, CancellationToken cancellationToken = default)
        {
            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            return await httpClient.PostAsync($"{WebApiUrl}/api/tokens", content, cancellationToken);  
        }

        public async Task<HttpResponseMessage> RegistrationAsync(RegistrationModel registration, CancellationToken cancellationToken = default)
        {
            var content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");
            return await httpClient.PostAsync($"{WebApiUrl}/api/user-registration", content, cancellationToken);
        }
    }
}
