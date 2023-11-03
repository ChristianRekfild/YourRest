using YourRest.Application.Dto.Models.RoomFacility;

namespace YourRest.Application.Interfaces.Facility
{
    public interface IGetAllRoomFacilitiesUseCase
    {
        Task<IEnumerable<RoomFacilityDto>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
