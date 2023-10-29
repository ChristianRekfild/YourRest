using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Interfaces.Room
{
    public interface IEditRoomUseCase
    {
        Task ExecuteAsync(RoomWithIdDto reviewDto);
    }
}
