using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models;
using YourRest.Application.Exceptions;
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
            var roomFacility = await roomFacilityRepository.GetAsync(id);
            if (roomFacility == null)
            {
                throw new EntityNotFoundException($"RoomFacility with id number {id} not found");
            }
            return roomFacility.ToViewModel();
        }
    }
}
