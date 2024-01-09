using YourRest.Application.Dto.Models.AccommodationFacility;

namespace YourRest.Application.Interfaces.AccommodationFacility
{
    public interface IAddAccommodationFacilityUseCase
    {
        Task ExecuteAsync(int accommodationId, AccommodationFacilityIdDto accommodationFacility, CancellationToken cancellationToken);
    }
}
