using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces.Facility;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.Facility
{
    public class GetRoomFacilityByIdUseCase : IGetRoomFacilityByIdUseCase
    {
        private readonly IRoomFacilityRepository roomFacilityRepository;

        public GetRoomFacilityByIdUseCase(IRoomFacilityRepository roomFacilityRepository)
        {
            this.roomFacilityRepository = roomFacilityRepository;
        }
        public async Task<RoomFacilityViewModel> ExecuteAsync(int id)
        {
            if (await roomFacilityRepository.GetAsync(id) is not RoomFacility roomFacility)
            {
                throw new Exception("Facility not found!");
            }
            return roomFacility.ToViewModel();
        }
    }
}
