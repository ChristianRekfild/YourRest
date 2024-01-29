using Newtonsoft.Json;
using System.Text;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services.Abstractions;

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
        public async Task<AddressModel> CreateAddressAsync(AddressModel address, int hotelId, CancellationToken cancellationToken = default)
        {
            var result = new AddressModel();
            var content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{WebApiUrl}/api/operators/accommodations/{hotelId}/address", content, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                result = await response.Content.ReadFromJsonAsync<AddressModel>(cancellationToken: cancellationToken);
            }
            return result;
        }

        public Task<AddressModel> FetchAddressByHotelIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AddressModel>> FetchAddressesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
