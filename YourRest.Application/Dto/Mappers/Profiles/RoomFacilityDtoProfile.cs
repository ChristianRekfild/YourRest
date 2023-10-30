using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class RoomFacilityDtoProfile : Profile
    {
        public RoomFacilityDtoProfile()
        {
            CreateMap<RoomFacility, RoomFacilityDto>()
             .ReverseMap();
        }
    }
}
