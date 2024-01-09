using Newtonsoft.Json;
using System.Text;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class HotelService : IHotelService
    {
        private readonly HttpClient httpClient;
        private readonly string WebApiUrl;

        public HotelService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClient = httpClientFactory.CreateClient();
            WebApiUrl = configuration.GetSection("webApiUrl").Value;
        }
        public async Task CreateHotel(HotelViewModel hotel)
        {
            var data = JsonConvert.SerializeObject(hotel);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            await httpClient.PostAsync($"{WebApiUrl}/api/accommodation",content);
        }
    }
}
