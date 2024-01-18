using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IHotelService
    {
        Task<HotelViewModel> CreateHotelAsync(HotelViewModel hotel, CancellationToken cancellationToken = default);
        Task<List<HotelViewModel>> FetchHotelsAsync(CancellationToken cancellationToken = default);
    }
}
