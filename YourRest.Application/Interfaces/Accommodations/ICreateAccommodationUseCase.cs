using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces.Accommodations
{
    public interface ICreateAccommodationUseCase
    {
        Task<AccommodationDto> ExecuteAsync(CreateAccommodationDto accommodationDto, string userKeyCloakId, CancellationToken cancellationToken);
    }
}
