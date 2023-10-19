using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces.Room
{
    public interface IGetRoomByIdUseCase
    {
        Task<RoomViewModel> ExecuteAsync(int id);
    }
}
