using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface ICreateRoomUseCase
    {
        Task<RoomWithIdDto> Execute(RoomDto roomDto, int accommodationId);
    }
}
