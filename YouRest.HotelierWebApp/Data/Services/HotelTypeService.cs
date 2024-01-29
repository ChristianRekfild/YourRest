using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services.Abstractions;

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
        public async Task<IEnumerable<HotelTypeModel>> FetchHotelTypesAsync(CancellationToken cancellationToken = default)
        {
           var response = await httpClient.GetAsync($"{WebApiUrl}/api/accommodation-types", cancellationToken);
           var hotelTypes = await response.Content.ReadFromJsonAsync<IEnumerable<HotelTypeModel>>(cancellationToken: cancellationToken);
            return hotelTypes;
        }
    }
}
