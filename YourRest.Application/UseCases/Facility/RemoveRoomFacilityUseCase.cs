using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Facility;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.Facility
{
    public class RemoveRoomFacilityUseCase : IRemoveRoomFacilityUseCase
    {
        private readonly IRoomFacilityRepository roomFacilityRepository;

        public RemoveRoomFacilityUseCase(IRoomFacilityRepository roomFacilityRepository)
        {
            this.roomFacilityRepository = roomFacilityRepository;
        }
        public async Task ExecuteAsync(int id)
        {
            if (await roomFacilityRepository.GetAsync(id) is not RoomFacility roomFacility)
            {
                throw new EntityNotFoundException($"RoomFacility with id number {id} not found");
            }
            await roomFacilityRepository.DeleteAsync(id);
        }
    }
}
