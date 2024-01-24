using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Interfaces
{
    public interface ICreateRoomUseCase
    {
        Task<RoomWithIdDto> ExecuteAsync(RoomDto roomDto, int accommodationId, CancellationToken cancellationToken);
    }
}
