using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface IGetRoomListUseCase
    {
        Task<IEnumerable<RoomWithIdDto>> Execute(int accommodationId);
    }
}
