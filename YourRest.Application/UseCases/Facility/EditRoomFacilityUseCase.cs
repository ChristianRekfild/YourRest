using AutoMapper;
using YourRest.Application.Dto.Models.RoomFacility;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Facility;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases.Facility
{
    public class EditRoomFacilityUseCase : IEditRoomFacilityUseCase
    {
        private readonly IRoomFacilityRepository roomFacilityRepository;
        private readonly IMapper mapper;

        public EditRoomFacilityUseCase(IRoomFacilityRepository roomFacilityRepository, IMapper mapper)
        {
            this.roomFacilityRepository = roomFacilityRepository;
            this.mapper = mapper;
        }
        public async Task ExecuteAsync(int roomFacilityId, Dto.Models.RoomFacility.RoomFacilityDto reviewDto, CancellationToken cancellationToken)
        {
            var roomFacilityWithDto = mapper.Map<RoomFacilityWithIdDto>(reviewDto);
            roomFacilityWithDto.Id = roomFacilityId;
            var roomFacility = await roomFacilityRepository.GetAsync(roomFacilityId, cancellationToken);
            if (roomFacility == null)
            {
                throw new EntityNotFoundException($"RoomFacility with id number {roomFacilityId} not found");
            }
            await roomFacilityRepository.UpdateAsync(mapper.Map<Infrastructure.Core.Contracts.Models.RoomFacilityDto>(roomFacilityWithDto), cancellationToken: cancellationToken);
        }
    }
}
