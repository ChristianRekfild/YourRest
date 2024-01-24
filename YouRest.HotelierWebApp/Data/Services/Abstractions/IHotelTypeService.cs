using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IHotelTypeService
    {
        Task<IEnumerable<HotelTypeViewModel>> FetchHotelTypesAsync(CancellationToken cancellationToken = default);
    }
}
