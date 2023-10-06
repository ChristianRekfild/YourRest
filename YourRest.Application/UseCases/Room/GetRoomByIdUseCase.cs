using YourRest.Application.CustomErrors;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
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
            if(await roomRepository.FindAsync(room => room.Id == id) is RoomEntity room) 
            {
                return room.ToViewModel();
            }
             throw new RoomNotFoundExeption(id);
        }
    }
}
