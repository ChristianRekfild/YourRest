using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Room;
using YourRest.Domain.Repositories;

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
            var room = await roomRepository.GetAsync(reviewDto.Id);
            if(room == null) 
            {
                throw new EntityNotFoundException($"Room with Id:{reviewDto.Id} not found");
            }
            await roomRepository.UpdateAsync(reviewDto.ToEntity());
        }
    }
}
