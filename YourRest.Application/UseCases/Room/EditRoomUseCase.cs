using AutoMapper;
using YourRest.Application.Dto.Models.Room;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Room;
using YourRest.Domain.Repositories;
using RoomEntity = YourRest.Domain.Entities.Room;

namespace YourRest.Application.UseCases.Room
{
    public class EditRoomUseCase : IEditRoomUseCase
    {
        private readonly IRoomRepository roomRepository;
        private readonly IMapper mapper;

        public EditRoomUseCase(IRoomRepository roomRepository, IMapper mapper)
        {
            this.roomRepository = roomRepository;
            this.mapper = mapper;
        }
        public async Task ExecuteAsync(RoomWithIdDto reviewDto)
        {
            var room = await roomRepository.GetAsync(reviewDto.Id);
            if(room == null) 
            {
                throw new EntityNotFoundException($"Room with Id:{reviewDto.Id} not found");
            }
            await roomRepository.UpdateAsync(mapper.Map<RoomEntity>(reviewDto));
        }
    }
}
