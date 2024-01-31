using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services.Abstractions;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class FileService : IFileService
    {
        private readonly HttpClient httpClient;
        private readonly string WebApiUrl;
        public FileService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            httpClient = httpClientFactory.CreateClient();
            WebApiUrl = configuration.GetSection("webApiUrl").Value;
        }

        public async Task<string> FetchImg(string filePath, string bucketType)
        {
            string baseUrl = $"{WebApiUrl}/api/photo/{filePath}";
            var param = new Dictionary<string, string>() { { "bucketType", "Accommodation" } };
            var url = new Uri(QueryHelpers.AddQueryString(baseUrl, param));
            var response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task UploadAsync(HotelImgModel hotelImg, CancellationToken cancellationToken = default)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(hotelImg), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{WebApiUrl}/api/accommodation-photo", content, cancellationToken);
        }
    }
}
