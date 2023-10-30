using AutoMapper;
using YourRest.Application.Dto.Mappers;
using YourRest.Application.Dto.Models.RoomFacility;
using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces.Facility;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Application.UseCases.Facility
{
    public class GetRoomFacilityByIdUseCase : IGetRoomFacilityByIdUseCase
    {
        private readonly IRoomFacilityRepository roomFacilityRepository;
        private readonly IMapper mapper;

        public GetRoomFacilityByIdUseCase(IRoomFacilityRepository roomFacilityRepository, IMapper mapper)
        {
            this.roomFacilityRepository = roomFacilityRepository;
            this.mapper = mapper;
        }
        public async Task<RoomFacilityWithIdDto> ExecuteAsync(int id)
        {
            var roomFacility = await roomFacilityRepository.GetAsync(id);
            if (roomFacility == null)
            {
                throw new EntityNotFoundException($"RoomFacility with id number {id} not found");
            }
            return mapper.Map<RoomFacilityWithIdDto>(roomFacility);
        }
    }
}
