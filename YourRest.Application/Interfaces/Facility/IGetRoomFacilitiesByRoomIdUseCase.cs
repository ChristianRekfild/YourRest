using YourRest.Application.Dto.Models.RoomFacility;

namespace YourRest.Application.Interfaces.Facility
{
    public interface IGetRoomFacilitiesByRoomIdUseCase
    {
        Task<IEnumerable<RoomFacilityDto>> ExecuteAsync(int roomId);
    }
}
