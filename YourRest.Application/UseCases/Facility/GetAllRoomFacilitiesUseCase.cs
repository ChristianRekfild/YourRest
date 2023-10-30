using AutoMapper;
using YourRest.Application.Dto.Models.RoomFacility;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Facility;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.Facility
{
    public class GetAllRoomFacilitiesUseCase : IGetAllRoomFacilitiesUseCase
    {
        private readonly IRoomFacilityRepository roomFacilityRepository;
        private readonly IMapper mapper;

        public GetAllRoomFacilitiesUseCase(IRoomFacilityRepository roomFacilityRepository, IMapper mapper)
        {
            this.roomFacilityRepository = roomFacilityRepository;
            this.mapper = mapper;
        }



        public async Task<IEnumerable<RoomFacilityDto>> ExecuteAsync()
        {
            var roomFacilities = await roomFacilityRepository.GetAllAsync();
            if (!roomFacilities.Any())
            {
                throw new EntityNotFoundException("Not found any RoomFacility");
            }
            return mapper.Map<IEnumerable<RoomFacilityDto>>(roomFacilities);
        }
    }
}
