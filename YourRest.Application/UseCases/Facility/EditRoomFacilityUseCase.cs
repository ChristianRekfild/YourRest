using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Facility;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.Facility
{
    public class EditRoomFacilityUseCase : IEditRoomFacilityUseCase
    {
        private readonly IRoomFacilityRepository roomFacilityRepository;
        public EditRoomFacilityUseCase(IRoomFacilityRepository roomFacilityRepository)
        {
            this.roomFacilityRepository = roomFacilityRepository;
        }
        public async Task ExecuteAsync(RoomFacilityViewModel reviewDto)
        {
            var roomFacility = await roomFacilityRepository.GetAsync(reviewDto.Id);
            if (roomFacility == null)
            {
                throw new EntityNotFoundException($"RoomFacility with id number {reviewDto.Id} not found");
            }
            if ((await roomFacilityRepository.FindAsync(rf => rf.RoomId == reviewDto.RoomId)).Select(f => f.Name).Contains(reviewDto.Name))
            {
                throw new EntityConflictException($"Room Facility \"{reviewDto.Name}\" has been in process");
            }
            await roomFacilityRepository.UpdateAsync(reviewDto.ToEntity());
        }
    }
}
