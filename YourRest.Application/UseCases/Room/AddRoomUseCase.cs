using YourRest.Application.CustomErrors;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Room;
using YourRest.Domain.Repositories;
using RoomEntity = YourRest.Domain.Entities.Room;

namespace YourRest.Application.UseCases.Room
{
    public class AddRoomUseCase : IAddRoomUseCase
    {
        private readonly IRoomRepository roomRepository;
        public AddRoomUseCase(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }
        public async Task ExecuteAsync(RoomViewModel reviewDto)
        {
            if (await roomRepository.FindAsync(room => room.Name == reviewDto.Name) is RoomEntity room)
            {
                throw new RoomAlreadyExistsException(reviewDto);
            }
            await roomRepository.AddAsync(reviewDto.ToEntity());
        }
    }
}
