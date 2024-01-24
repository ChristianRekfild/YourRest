using BlazorBootstrap;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using System.Text;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class HotelService : IHotelService
    {
        private readonly HttpClient httpClient;
        private readonly string WebApiUrl;
        private readonly ProtectedLocalStorage localStorage;
        private readonly PreloadService preloadService;

        public HotelService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ProtectedLocalStorage localStorage, PreloadService preloadService)
        {

            httpClient = httpClientFactory.CreateClient();
            this.localStorage = localStorage;
            this.preloadService = preloadService;
            WebApiUrl = configuration.GetSection("webApiUrl").Value;
        }

        public async Task<HotelModel> CreateHotelAsync(HotelModel hotel, CancellationToken cancellationToken = default)
        {
            await httpClient.SetAccessToken(localStorage);
            var content = new StringContent(JsonConvert.SerializeObject(hotel), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{WebApiUrl}/api/accommodation", content, cancellationToken);
            return await response.Content.ReadFromJsonAsync<HotelModel>(cancellationToken: cancellationToken);

        }

        public async Task<List<HotelModel>> FetchHotelsAsync(CancellationToken cancellationToken = default)
        {
            preloadService.Show(SpinnerColor.Light, "Загружаем ваши отели...");
            await Task.Delay(1000); // call the service/api
            await httpClient.SetAccessToken(localStorage);
            List<HotelModel>? result = new();
            var content = new StringContent(JsonConvert.SerializeObject(new FetchHotelsModel()), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{WebApiUrl}/api/accommodations", content, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result = await response.Content.ReadFromJsonAsync<List<HotelModel>>(cancellationToken: cancellationToken);
            }
            preloadService.Hide();
            return result;
        }
    }
}
