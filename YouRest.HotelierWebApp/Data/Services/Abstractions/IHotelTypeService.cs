using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IHotelTypeService
    {
        Task<IEnumerable<HotelTypeModel>> FetchHotelTypesAsync(CancellationToken cancellationToken = default);
    }
}
