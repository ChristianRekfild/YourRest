using System.Security.AccessControl;
using YouRest.HotelierWebApp.Data.Models;
using YouRest.HotelierWebApp.Data.Services.Abstractions;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient httpClient;
        private readonly string WebApiUrl;

        public CountryService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClient = httpClientFactory.CreateClient();
            WebApiUrl = configuration.GetSection("webApiUrl").Value;
        }
        public async Task<IEnumerable<CountryModel>> FetchCountriesAsync(CancellationToken cancellationToken = default)
        {
            
            var response = await httpClient.GetAsync($"{WebApiUrl}/api/countries", cancellationToken);
            var countries = await response.Content.ReadFromJsonAsync<IEnumerable<CountryModel>>();
            return countries;
        }

        public async Task<CountryModel> FetchCountryAsync(int id, CancellationToken cancellationToken = default)
        {
            var countries = await FetchCountriesAsync(cancellationToken);
            return countries.FirstOrDefault(x => x.Id == id);
        }
    }
}
