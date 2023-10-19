using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Facility
{
    public interface IGetRoomFacilityByIdUseCase
    {
        Task<RoomFacilityViewModel> ExecuteAsync(int id);
    }
}
