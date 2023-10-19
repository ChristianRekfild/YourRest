using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Room
{
    public interface IEditRoomUseCase
    {
        Task ExecuteAsync(RoomViewModel reviewDto);
    }
}
