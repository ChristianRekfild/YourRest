using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Interfaces
{
    public interface IGetRoomListUseCase
    {
        Task<IEnumerable<RoomWithIdDto>> ExecuteAsync(int accommodationId, CancellationToken cancellationToken);
    }
}
