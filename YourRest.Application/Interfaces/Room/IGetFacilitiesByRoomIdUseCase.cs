using YourRest.Application.Dto.Models.RoomFacility;

namespace YourRest.Application.Interfaces.Room
{
    public interface IGetFacilitiesByRoomIdUseCase
    {
        Task<IEnumerable<RoomFacilityDto>> ExecuteAsync(int roomId);
    }
}
