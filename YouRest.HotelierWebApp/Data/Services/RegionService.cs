using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class RegionService : IRegionService
    {
        private readonly HttpClient httpClient;

        public RegionService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
        }
        public async Task<IEnumerable<RegionViewModel>> FetchRegionsAsync(CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync("http://localhost:52893/api/regions", cancellationToken);
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
