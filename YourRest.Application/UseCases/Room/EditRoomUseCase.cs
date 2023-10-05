using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Room;
using YourRest.Domain.Repositories;
using RoomEntity = YourRest.Domain.Entities.Room;

namespace YourRest.Application.UseCases.Room
{
    public class EditRoomUseCase : IEditRoomUseCase
    {
        private readonly IRoomRepository roomRepository;
        public EditRoomUseCase(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }
        public async Task ExecuteAsync(RoomViewModel reviewDto)
        {
            if(await roomRepository.FindAsync(room => room.Id == reviewDto.Id) is not RoomEntity room) 
            {
                throw new Exception("Room not found!");
            }
            await roomRepository.UpdateAsync(reviewDto.ToEntity());
        }
    }
}
