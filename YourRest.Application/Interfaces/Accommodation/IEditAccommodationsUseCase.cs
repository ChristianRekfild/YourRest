using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;

namespace YourRest.Application.Interfaces
{
    public interface IEditAccommodationsUseCase
    {
        Task<AccommodationExtendedDto> ExecuteAsync(AccommodationExtendedDto accommodationExtendedDto, CancellationToken cancellationToken);
    }
}
