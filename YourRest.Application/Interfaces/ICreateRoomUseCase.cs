using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Interfaces
{
    public interface ICreateRoomUseCase
    {
        Task<RoomWithIdDto> Execute(RoomDto roomDto, int accommodationId);
    }
}
