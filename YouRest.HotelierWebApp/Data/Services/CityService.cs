using System.Net.Http;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services.Abstractions;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class CityService : ICityService
    {
        private readonly HttpClient httpClient;
        private readonly string WebApiUrl;

        public CityService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            httpClient = httpClientFactory.CreateClient();
            WebApiUrl = configuration.GetSection("webApiUrl").Value;
        }
        public async Task<CityModel> FetchCytiAsync(int id, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"{WebApiUrl}/api/cities/{id}", cancellationToken);
            return await response.Content.ReadFromJsonAsync<CityModel>();
        }

        public async Task<IEnumerable<CityModel>> FetchCytiesAsync(CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"{WebApiUrl}/api/cities", cancellationToken);
            return await response.Content.ReadFromJsonAsync<IEnumerable<CityModel>>();
        }
    }
}
