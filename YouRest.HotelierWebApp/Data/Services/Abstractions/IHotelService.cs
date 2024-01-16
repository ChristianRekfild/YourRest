using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IHotelService
    {
        Task CreateHotel(HotelViewModel hotel);
        Task<List<CountryViewModel>> FetchHotelsAsync();
    }
}
