using YouRest.HotelierWebApp.Data.Services.Abstractions;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient httpClient;

        public CountryService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
        }
        public async Task<IEnumerable<CountryViewModel>> FetchCountriesAsync(CancellationToken cancellationToken)
        {
            
            var response = await httpClient.GetAsync("http://localhost:52893/api/countries", cancellationToken);
            var countries = await response.Content.ReadFromJsonAsync<IEnumerable<CountryViewModel>>();
            return countries;
        }

        public async Task<CountryViewModel> FetchCountryAsync(int id, CancellationToken cancellationToken)
        {
            var countries = await FetchCountriesAsync(cancellationToken);
            return countries.FirstOrDefault(x => x.Id == id);
        }
    }
}
