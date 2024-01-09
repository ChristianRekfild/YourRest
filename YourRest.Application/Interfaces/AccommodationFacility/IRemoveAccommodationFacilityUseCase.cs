using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.AccommodationFacility
{
    public interface IRemoveAccommodationFacilityUseCase
    {
        Task ExecuteAsync(int id, int facilityId, CancellationToken cancellationToken);
    }
}
