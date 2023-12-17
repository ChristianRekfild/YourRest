using YourRest.Application.Dto.Models.AccommodationFacility;

namespace YourRest.Application.Interfaces.AccommodationFacility
{
    public interface IGetAccommodationFacilityByAccommodationIdUseCase
    {
        Task<IEnumerable<AccommodationFacilityWithIdDto>> ExecuteAsync(int accommodationId, CancellationToken cancellationToken);
    }
}
