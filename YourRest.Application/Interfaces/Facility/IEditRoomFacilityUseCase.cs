using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Facility
{
    public interface IEditRoomFacilityUseCase
    {
        Task ExecuteAsync(RoomFacilityViewModel reviewDto);
    }
}
