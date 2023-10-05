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
            if (roomRepository.GetWithIncludeAsync(r => r.Id == reviewDto.RoomId, include => include.RoomFacilities).Result.FirstOrDefault() is not RoomEntity room)
            {
                throw new Exception("Room not found!");
            }
            if (room.RoomFacilities.Select(f => f.Name).Contains(reviewDto.Name))
            {
                throw new Exception("Facility is alredy exists!");
            }
            await roomFacilityRepository.AddAsync(reviewDto.ToEntity());
        }
    }
}
