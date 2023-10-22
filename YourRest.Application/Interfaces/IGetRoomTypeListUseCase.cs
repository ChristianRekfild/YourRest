using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface IGetRoomTypeListUseCase
    {
        Task<IEnumerable<RoomTypeDto>> Execute();
    }
}
