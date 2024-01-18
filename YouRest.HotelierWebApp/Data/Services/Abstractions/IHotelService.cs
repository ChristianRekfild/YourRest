using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IHotelService
    {
        Task<HotelViewModel> CreateHotel(HotelViewModel hotel, CancellationToken cancellationToken);
        Task<List<HotelViewModel>> FetchHotelsAsync(CancellationToken cancellationToken);
    }
}
