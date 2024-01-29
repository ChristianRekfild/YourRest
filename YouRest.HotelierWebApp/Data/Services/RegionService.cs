using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services.Abstractions;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class RegionService : IRegionService
    {
        private readonly HttpClient httpClient;
        private readonly string webApiUrl;
        public RegionService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClient = httpClientFactory.CreateClient();
            webApiUrl = configuration.GetSection("WebApiUrl").Value;
        }
        public async Task<IEnumerable<RegionModel>> FetchRegionsAsync(CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"{webApiUrl}/api/regions", cancellationToken);
            var regions = await response.Content.ReadFromJsonAsync<IEnumerable<RegionModel>>(cancellationToken: cancellationToken);
            return regions;
        }

        public async Task<RegionModel> FetchRegionAsync(int id, CancellationToken cancellationToken = default)
        {
            var regions = await FetchRegionsAsync(cancellationToken);
            return regions.FirstOrDefault(x => x.Id == id);
        }
    }
}
