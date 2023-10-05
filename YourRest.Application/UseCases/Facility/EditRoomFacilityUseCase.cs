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
            if (await roomFacilityRepository.FindAsync(f => f.Id == reviewDto.Id) is not RoomFacilityEntity roomFacility)
            {
                throw new Exception("roo facility is not found!");
            }
            await roomFacilityRepository.UpdateAsync(reviewDto.ToEntity());
        }
    }
}
