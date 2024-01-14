using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;

namespace YourRest.Application.Interfaces.Accommodations
{
    public interface IGetAccommodationsUseCase
    {
        Task<List<AccommodationExtendedDto>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
