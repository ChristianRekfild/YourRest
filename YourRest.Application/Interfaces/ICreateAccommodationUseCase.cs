using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface ICreateAccommodationUseCase
    {
        Task<AccommodationDto> ExecuteAsync(CreateAccommodationDto accommodationDto, CancellationToken cancellationToken);
    }
}
