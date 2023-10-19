using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Facility
{
    public interface IGetRoomFacilitiesByRoomIdUseCase
    {
        Task<IEnumerable<RoomFacilityViewModel>> ExecuteAsync(int roomId);
    }
}
