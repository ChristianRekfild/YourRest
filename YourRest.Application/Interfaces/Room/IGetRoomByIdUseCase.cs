using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Interfaces.Room
{
    public interface IGetRoomByIdUseCase
    {
        Task<RoomWithIdDto> ExecuteAsync(int id);
    }
}
