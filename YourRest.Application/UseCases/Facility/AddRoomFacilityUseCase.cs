using YourRest.Application.CustomErrors;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Facility;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using RoomEntity = YourRest.Domain.Entities.Room;

namespace YourRest.Application.UseCases.Facility
{
    public class AddRoomFacilityUseCase : IAddRoomFacilityUseCase
    {
        private readonly IRoomFacilityRepository roomFacilityRepository;
        private readonly IRoomRepository roomRepository;

        public AddRoomFacilityUseCase(IRoomFacilityRepository roomFacilityRepository, IRoomRepository roomRepository)
        {
            this.roomFacilityRepository = roomFacilityRepository;
            this.roomRepository = roomRepository;
        }
        public async Task ExecuteAsync(RoomFacilityViewModel reviewDto)
        {
            if (await roomRepository.GetAsync(reviewDto.RoomId) is not RoomEntity room)
            {
                throw new RoomNotFoundExeption(reviewDto.RoomId);
            }
            if (roomFacilityRepository.FindAsync(rf => rf.RoomId == reviewDto.RoomId).Result.Select(rf => rf.Name).Contains(reviewDto.Name))
            {
                throw new RoomFacilityInProcessException(reviewDto);
            }
            await roomFacilityRepository.AddAsync(reviewDto.ToEntity());
        }
    }
}
