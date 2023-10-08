using YourRest.Application.CustomErrors;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Facility;
using YourRest.Domain.Entities;
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
            if (await roomFacilityRepository.FindAsync(f => f.Id == reviewDto.Id) is not RoomFacility roomFacility)
            {
                throw new RoomFacilityNotFoundException(reviewDto.Id);
            }
            if (roomFacilityRepository.FindAsync(rf => rf.RoomId == reviewDto.RoomId).Result.Select(f => f.Name).Contains(reviewDto.Name))
            {
                throw new RoomFacilityInProcessException(reviewDto);
            }
            await roomFacilityRepository.UpdateAsync(reviewDto.ToEntity());
        }
    }
}
