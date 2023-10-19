using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Room
{
    public interface IGetFacilitiesByRoomIdUseCase
    {
        Task<IEnumerable<RoomFacilityViewModel>> ExecuteAsync(int roomId);
    }
}
