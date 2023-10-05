using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Room
{
    public interface IAddRoomUseCase
    {
        Task ExecuteAsync(RoomViewModel reviewDto);
    }
}
