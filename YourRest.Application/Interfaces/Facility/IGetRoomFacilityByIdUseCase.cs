using YourRest.Application.Dto.Models.RoomFacility;

namespace YourRest.Application.Interfaces.Facility
{
    public interface IGetRoomFacilityByIdUseCase
    {
        Task<RoomFacilityWithIdDto> ExecuteAsync(int id);
    }
}
