using Newtonsoft.Json;
using System.Text;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient httpClient;
        private readonly string WebApiUrl;

        public AddressService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClient = httpClientFactory.CreateClient();
            WebApiUrl = configuration.GetSection("webApiUrl").Value;
        }
        public async Task<AddressViewModel> CreateAddressAsync(AddressViewModel address, int hotelId, CancellationToken cancellationToken = default)
        {
            var result = new AddressViewModel();
            var content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{WebApiUrl}/api/operators/accommodations/{hotelId}/address", content, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                result = await response.Content.ReadFromJsonAsync<AddressViewModel>(cancellationToken: cancellationToken);
            }
            return result;
        }

        public Task<AddressViewModel> FetchAddressByHotelIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AddressViewModel>> FetchAddressesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
