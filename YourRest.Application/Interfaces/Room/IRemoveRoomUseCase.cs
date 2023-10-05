using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Room
{
    public interface IRemoveRoomUseCase
    {
        Task ExecuteAsync(RoomViewModel reviewDto);
    }
}
