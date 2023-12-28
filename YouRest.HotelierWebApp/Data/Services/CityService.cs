using System.Net.Http;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class CityService : ICityService
    {
        private readonly HttpClient httpClient;

        public CityService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
        }
        public async Task<CityViewModel> FetchCytiAsync(int id, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"http://localhost:52893/api/cities/{id}", cancellationToken);
            return await response.Content.ReadFromJsonAsync<CityViewModel>();
        }

        public async Task<IEnumerable<CityViewModel>> FetchCytiesAsync(CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync("http://localhost:52893/api/cities", cancellationToken);
            return await response.Content.ReadFromJsonAsync<IEnumerable<CityViewModel>>();
        }
    }
}
