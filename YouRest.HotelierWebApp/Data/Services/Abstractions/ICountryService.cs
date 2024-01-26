using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryViewModel>> FetchCountriesAsync(CancellationToken cancellationToken = default);
        Task<CountryViewModel> FetchCountryAsync(int id, CancellationToken cancellationToken = default);
    }
}
