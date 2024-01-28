using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;

namespace YourRest.Application.Interfaces.Accommodations
{
    public interface IGetAccommodationsUseCase
    {
        Task<IEnumerable<AccommodationExtendedDto>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
