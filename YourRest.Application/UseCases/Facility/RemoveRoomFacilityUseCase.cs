using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Facility;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases.Facility
{
    public class RemoveRoomFacilityUseCase : IRemoveRoomFacilityUseCase
    {
        private readonly IRoomFacilityRepository roomFacilityRepository;

        public RemoveRoomFacilityUseCase(IRoomFacilityRepository roomFacilityRepository)
        {
            this.roomFacilityRepository = roomFacilityRepository;
        }
        public async Task ExecuteAsync(int id, CancellationToken cancellationToken)
        {
            var roomFacility = await roomFacilityRepository.GetAsync(id, cancellationToken);
            if (roomFacility == null)
            {
                throw new EntityNotFoundException($"RoomFacility with id number {id} not found");
            }
            await roomFacilityRepository.DeleteAsync(id, cancellationToken: cancellationToken);
        }
    }
}
