using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;

namespace YourRest.Application.Interfaces.Accommodations
{
    public interface IEditAccommodationsUseCase
    {
        Task<AccommodationDto> ExecuteAsync(EditAccommodationDto accommodation, CancellationToken cancellationToken);
    }
}
