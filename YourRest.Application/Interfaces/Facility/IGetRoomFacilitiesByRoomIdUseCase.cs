using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Facility
{
    public interface IGetRoomFacilitiesByRoomIdUseCase
    {
        Task<IEnumerable<RoomFacilityDto>> ExecuteAsync(int roomId);
    }
}
