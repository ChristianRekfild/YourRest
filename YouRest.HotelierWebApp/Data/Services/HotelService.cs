using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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
        private readonly IAuthorizationService authorizationService;

        public HotelService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ProtectedSessionStorage storage, IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
            httpClient = httpClientFactory.CreateClient();
            this.storage = storage;
            WebApiUrl = configuration.GetSection("webApiUrl").Value;
        }
        public async Task<HotelViewModel> CreateHotel(HotelViewModel hotel, CancellationToken cancellationToken)
        {
            var result = new HotelViewModel();
            var accessToken = (await storage.GetAsync<string>("accessToken")).Value;
            httpClient.SetBearerToken(accessToken);
            var content = new StringContent(JsonConvert.SerializeObject(hotel), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{WebApiUrl}/api/accommodation", content, cancellationToken);
            result = await response.Content.ReadFromJsonAsync<HotelViewModel>(cancellationToken: cancellationToken);


            return result;
        }

        public async Task<List<HotelViewModel>> FetchHotelsAsync(CancellationToken cancellationToken)
        {
            List<HotelViewModel>? result = new();
            var accessToken = (await storage.GetAsync<string>("accessToken")).Value;
            httpClient.SetBearerToken(accessToken);
            var data = JsonConvert.SerializeObject(new FetchHotelsViewModel());
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{WebApiUrl}/api/accommodations",content,cancellationToken);
            if(response.StatusCode == System.Net.HttpStatusCode.OK) 
            {
              result = await response.Content.ReadFromJsonAsync<List<HotelViewModel>>(cancellationToken: cancellationToken);
            }
            return result;
        }
    }
}
