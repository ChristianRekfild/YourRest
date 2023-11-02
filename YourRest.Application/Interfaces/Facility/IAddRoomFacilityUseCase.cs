using YourRest.Application.Dto.Models.RoomFacility;

namespace YourRest.Application.Interfaces.Facility
{
    public interface IAddRoomFacilityUseCase
    {
        Task ExecuteAsync(int roomId, IEnumerable<RoomFacilityDto> reviewDto, CancellationToken cancellationToken);
    }
}
