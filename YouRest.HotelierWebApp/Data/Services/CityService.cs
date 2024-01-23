using System.Net.Http;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

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
        public async Task<CityViewModel> FetchCytiAsync(int id, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"{WebApiUrl}/api/cities/{id}", cancellationToken);
            return await response.Content.ReadFromJsonAsync<CityViewModel>();
        }

        public async Task<IEnumerable<CityViewModel>> FetchCytiesAsync(CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"{WebApiUrl}/api/cities", cancellationToken);
            return await response.Content.ReadFromJsonAsync<IEnumerable<CityViewModel>>();
        }
    }
}
