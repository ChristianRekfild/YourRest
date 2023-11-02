using YourRest.Application.Dto.Models.RoomFacility;

namespace YourRest.Application.Interfaces.Facility
{
    public interface IEditRoomFacilityUseCase
    {
        Task ExecuteAsync(int roomFacilityId, RoomFacilityDto reviewDto, CancellationToken cancellationToken);
    }
}
