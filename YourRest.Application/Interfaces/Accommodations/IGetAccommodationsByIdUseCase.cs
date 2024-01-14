using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;

namespace YourRest.Application.Interfaces.Accommodations
{
    public interface IGetAccommodationsByIdUseCase
    {
        Task<AccommodationExtendedDto> ExecuteAsync(int accommodationId, CancellationToken cancellationToken);
    }
}
