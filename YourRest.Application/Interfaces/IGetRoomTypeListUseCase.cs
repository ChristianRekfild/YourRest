using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IGetRoomTypeListUseCase
    {
        Task<IEnumerable<RoomTypeDto>> Execute();
    }
}
