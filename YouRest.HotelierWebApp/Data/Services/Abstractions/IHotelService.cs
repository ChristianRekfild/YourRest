using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IHotelService
    {
        Task<HotelModel> CreateHotelAsync(HotelModel hotel, CancellationToken cancellationToken = default);
        Task<List<HotelModel>> FetchHotelsAsync(CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> RemoveHotelAsync(int addressId, int hotelId, CancellationToken cancellationToken = default);
        int ConvertHotelRating(string ratingValue);
        string ConvertHotelRating(int ratingValue);
    }
}
