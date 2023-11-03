using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Interfaces.Room
{
    public interface IGetRoomByIdUseCase
    {
        Task<RoomExtendedDto> ExecuteAsync(int id, CancellationToken cancellationToken);
    }
}
