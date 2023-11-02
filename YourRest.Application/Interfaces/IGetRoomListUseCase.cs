using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Interfaces
{
    public interface IGetRoomListUseCase
    {
        Task<IEnumerable<RoomWithIdDto>> Execute(int accommodationId, CancellationToken cancellationToken);
    }
}
