using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

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

        public async Task Upload(IBrowserFile file)
        {
            var data = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(data);
            //MemoryStream stream = new MemoryStream(data);
            //IFormFile _file = new FormFile(stream, 0, file.Size, file.Name, file.Name);
            HotelImgViewModel hotel = new()
            {
                AccommodationId = 3,
                Photo = $"{Convert.ToBase64String(data)}",
                FileName = file.Name
            };
            //HttpContent streamContent = new StreamContent(file.OpenReadStream());
            //streamContent.Headers.ContentDisposition = new ("form-data")
            //{
            //    Name = "model",
            //    FileName = file.Name,
            //};

            //var data = new HotelImgViewModel()
            //{
            //    AccommodationId = 1,
            //    Photo = img
            //};
            var a = JsonConvert.SerializeObject(hotel);
            HttpContent content = new StringContent(a, Encoding.UTF8, "application/json");
            //streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            //using var formData = new MultipartFormDataContent();
            //formData.Add(streamContent, "model", file.Name);

            var response = await httpClient.PostAsync($"{WebApiUrl}/api/accommodation-photo", content);
        }
    }
}
