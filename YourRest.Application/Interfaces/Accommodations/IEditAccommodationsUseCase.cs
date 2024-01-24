using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;

namespace YourRest.Application.Interfaces.Accommodations
{
    public interface IEditAccommodationsUseCase
    {
        Task<AccommodationExtendedDto> ExecuteAsync(CreateAccommodationDto accommodation, int id, CancellationToken cancellationToken);
    }
}
