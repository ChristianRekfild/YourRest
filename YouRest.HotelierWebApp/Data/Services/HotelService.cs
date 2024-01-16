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
        public async Task CreateHotel(HotelViewModel hotel)
        {
            var responseMessage = await authorizationService.LoginAsync(new AuthorizationViewModel() { Password = "123456", Username = "grits@gmail.com" });
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var token = await responseMessage.Content.ReadFromJsonAsync<TokenViewModel>();
                httpClient.SetBearerToken(token.AccessToken);
                var data = JsonConvert.SerializeObject(hotel);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var res = await httpClient.PostAsync($"{WebApiUrl}/api/accommodation", content);
            }
            //var accessToken = (await storage.GetAsync<string>("accessToken")).Value;

        }

        public async Task<List<CountryViewModel>> FetchHotelsAsync()
        {
            var response = await httpClient.GetAsync($"{WebApiUrl}/api/accommodation");
            return await response.Content.ReadFromJsonAsync<List<CountryViewModel>>();
        }
    }
}
