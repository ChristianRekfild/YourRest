using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IAddressService
    {
        Task<AddressModel> CreateAddressAsync (AddressModel address, int hotelId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AddressModel>> FetchAddressesAsync(CancellationToken cancellationToken = default);
        Task<AddressModel> FetchAddressByHotelIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
