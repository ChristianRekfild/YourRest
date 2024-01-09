using YourRest.Application.Dto.Models.AccommodationFacility;

namespace YourRest.Application.Interfaces.AccommodationFacility
{
    public interface IGetAllAccommodationFacilitiesUseCase
    {
        Task<IEnumerable<AccommodationFacilityWithIdDto>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
