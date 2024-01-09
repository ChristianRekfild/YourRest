using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

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
        public async Task<IEnumerable<RegionViewModel>> FetchRegionsAsync(CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"{webApiUrl}/api/regions", cancellationToken);
            var regions = await response.Content.ReadFromJsonAsync<IEnumerable<RegionViewModel>>();
            return regions;
        }

        public async Task<RegionViewModel> FetchRegionAsync(int id, CancellationToken cancellationToken = default)
        {
            var regions = await FetchRegionsAsync(cancellationToken);
            return regions.FirstOrDefault(x => x.Id == id);
        }
    }
}
