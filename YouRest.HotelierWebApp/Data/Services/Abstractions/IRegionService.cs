using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionViewModel>> FetchRegionsAsync(CancellationToken cancellationToken = default);
        Task<RegionViewModel> FetchRegionAsync(int id, CancellationToken cancellationToken = default);
    }
}
