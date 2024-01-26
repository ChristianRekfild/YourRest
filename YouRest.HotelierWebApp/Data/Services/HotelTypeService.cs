using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class HotelTypeService : IHotelTypeService
    {
        private readonly HttpClient httpClient;
        private readonly string WebApiUrl;

        public HotelTypeService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClient = httpClientFactory.CreateClient();
            WebApiUrl = configuration.GetSection("webApiUrl").Value;
        }
        public async Task<IEnumerable<HotelTypeViewModel>> FetchHotelTypesAsync(CancellationToken cancellationToken = default)
        {
           var response = await httpClient.GetAsync($"{WebApiUrl}/api/accommodation-types", cancellationToken);
           var hotelTypes = await response.Content.ReadFromJsonAsync<IEnumerable<HotelTypeViewModel>>(cancellationToken: cancellationToken);
            return hotelTypes;
        }
    }
}
