using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IAddressService
    {
        Task<AddressViewModel> CreateAddressAsync (AddressViewModel address, int hotelId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AddressViewModel>> FetchAddressesAsync(CancellationToken cancellationToken = default);
        Task<AddressViewModel> FetchAddressByHotelIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
