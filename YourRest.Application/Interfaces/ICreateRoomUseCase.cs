using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface ICreateRoomUseCase
    {
        Task<RoomDto> Execute(RoomDto roomDto);
    }
}
