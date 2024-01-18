using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IAddressService
    {
        Task<AddressViewModel> CreateAddress (AddressViewModel address, int hotelId);
        Task<IEnumerable<AddressViewModel>> FetchAddresses();
        Task<AddressViewModel> FetchAddressByHotelId(int id);
    }
}
