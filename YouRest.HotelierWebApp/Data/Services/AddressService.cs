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
        public async Task<AddressViewModel> CreateAddress(AddressViewModel address, int hotelId)
        {
            var result = new AddressViewModel();
            var content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{WebApiUrl}/api/operators/accommodations/{hotelId}/address", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = await response.Content.ReadFromJsonAsync<AddressViewModel>();
            }
            return result;
        }

        public Task<AddressViewModel> FetchAddressByHotelId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AddressViewModel>> FetchAddresses()
        {
            throw new NotImplementedException();
        }
    }
}
