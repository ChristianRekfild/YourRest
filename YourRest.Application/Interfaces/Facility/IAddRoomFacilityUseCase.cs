using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Facility
{
    public interface IAddRoomFacilityUseCase
    {
        Task ExecuteAsync(RoomFacilityDto reviewDto);
    }
}
