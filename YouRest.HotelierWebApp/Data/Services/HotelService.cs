using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
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
        private readonly ProtectedSessionStorage storage;


        public HotelService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ProtectedSessionStorage storage)
        {
            httpClient = httpClientFactory.CreateClient();
            this.storage = storage;
            WebApiUrl = configuration.GetSection("webApiUrl").Value;
        }

        public async Task<HotelViewModel> CreateHotelAsync(HotelViewModel hotel, CancellationToken cancellationToken = default)
        {
            await httpClient.SetAccessToken(storage);
            var content = new StringContent(JsonConvert.SerializeObject(hotel), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{WebApiUrl}/api/accommodation", content, cancellationToken);
            return await response.Content.ReadFromJsonAsync<HotelViewModel>(cancellationToken: cancellationToken);

        }

        public async Task<List<HotelViewModel>> FetchHotelsAsync(CancellationToken cancellationToken = default)
        {
            await httpClient.SetAccessToken(storage);
            List<HotelViewModel>? result = new();
            var content = new StringContent(JsonConvert.SerializeObject(new FetchHotelsViewModel()), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{WebApiUrl}/api/accommodations", content, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = await response.Content.ReadFromJsonAsync<List<HotelViewModel>>(cancellationToken: cancellationToken);
            }
            return result;
        }
    }
}
