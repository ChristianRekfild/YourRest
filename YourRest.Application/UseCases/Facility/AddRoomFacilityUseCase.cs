using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Facility;
using YourRest.Domain.Repositories;

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
            var room = await roomRepository.GetAsync(reviewDto.RoomId);
            if (room == null)
            {
                throw new EntityNotFoundException($"Room with id number {reviewDto.RoomId} not found");
            }

            if ((await roomFacilityRepository.FindAsync(rf => rf.RoomId == reviewDto.RoomId)).Select(rf => rf.Name).Contains(reviewDto.Name))
            {
                throw new EntityConflictException($"Room Facility \"{reviewDto.Name}\" has been in process");
            }
            await roomFacilityRepository.AddAsync(reviewDto.ToEntity());
        }
    }
}
