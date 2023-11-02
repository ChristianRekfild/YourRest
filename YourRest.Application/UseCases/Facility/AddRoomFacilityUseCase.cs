using AutoMapper;
using YourRest.Application.Dto.Models.RoomFacility;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Facility;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.Facility
{
    public class AddRoomFacilityUseCase : IAddRoomFacilityUseCase
    {
        private readonly IMapper mapper;
        private readonly IRoomFacilityRepository roomFacilityRepository;
        private readonly IRoomRepository roomRepository;

        public AddRoomFacilityUseCase(
            IMapper mapper,
            IRoomFacilityRepository roomFacilityRepository,
            IRoomRepository roomRepository)
        {
            this.mapper = mapper;
            this.roomFacilityRepository = roomFacilityRepository;
            this.roomRepository = roomRepository;
        }
        public async Task ExecuteAsync(int roomId, IEnumerable<RoomFacilityDto> roomFacilities, CancellationToken cancellationToken)
        {
            var room = (await roomRepository.GetWithIncludeAsync(room => room.Id == roomId, cancellationToken, include => include.RoomFacilities)).FirstOrDefault();
            if (room == null)
            {
                throw new EntityNotFoundException($"Room with id number {roomId} not found");
            }
            foreach (var roomFacility in roomFacilities)
            {
                if (room.RoomFacilities.Select(rf => rf.Name).Contains(roomFacility.Name))
                {
                    throw new EntityConflictException($"Room Facility \"{roomFacility.Name}\" has been in process");
                }
                room.RoomFacilities.Add(mapper.Map<RoomFacility>(roomFacilities));
            }
            await roomFacilityRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
