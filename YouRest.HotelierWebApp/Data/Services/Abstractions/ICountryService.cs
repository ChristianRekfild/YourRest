using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryModel>> FetchCountriesAsync(CancellationToken cancellationToken = default);
        Task<CountryModel> FetchCountryAsync(int id, CancellationToken cancellationToken = default);
    }
}
