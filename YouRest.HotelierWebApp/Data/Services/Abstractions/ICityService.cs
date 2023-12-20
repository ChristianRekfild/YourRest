using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface ICityService
    {
        Task<IEnumerable<CityViewModel>> FetchCytiesAsync(CancellationToken cancellationToken = default);
        Task<CityViewModel> FetchCytiAsync(int id, CancellationToken cancellationToken = default);
    }
}
