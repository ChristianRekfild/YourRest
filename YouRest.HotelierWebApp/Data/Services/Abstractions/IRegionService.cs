using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionModel>> FetchRegionsAsync(CancellationToken cancellationToken = default);
        Task<RegionModel> FetchRegionAsync(int id, CancellationToken cancellationToken = default);
    }
}
