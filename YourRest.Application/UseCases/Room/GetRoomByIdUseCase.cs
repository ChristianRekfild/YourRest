using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Room;
using YourRest.Domain.Repositories;
using RoomEntity = YourRest.Domain.Entities.Room;

namespace YourRest.Application.UseCases.Room
{
    public class GetRoomByIdUseCase : IGetRoomByIdUseCase
    {
        private readonly IRoomRepository roomRepository;
        public GetRoomByIdUseCase(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }
        public async Task<RoomViewModel> ExecuteAsync(int id)
        {
            if(await roomRepository.GetAsync(id) is RoomEntity room)
            {
                return room.ToViewModel();
            }
            throw new EntityNotFoundException($"Room with Id:{id} not found");
        }
    }
}
