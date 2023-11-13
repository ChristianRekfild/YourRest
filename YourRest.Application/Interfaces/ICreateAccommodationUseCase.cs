using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface ICreateAccommodationUseCase
    {
        Task<AccommodationDto> Execute(CreateAccommodationDto accommodationDto, CancellationToken cancellationToken);
    }
}
