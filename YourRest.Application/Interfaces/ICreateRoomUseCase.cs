using YourRest.Application.Dto;

namespace YourRest.Application.Interfaces
{
    public interface ICreateRoomUseCase
    {
        Task<SavedRoomDto> Execute(SavedRoomDto roomDto);
    }
}
