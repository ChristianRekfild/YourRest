using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface ICityService
    {
        Task<IEnumerable<CityModel>> FetchCytiesAsync(CancellationToken cancellationToken = default);
        Task<CityModel> FetchCytiAsync(int id, CancellationToken cancellationToken = default);
    }
}
