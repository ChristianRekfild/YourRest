using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using YourRest.Application.Dto.Models.RoomFacility;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class RoomFacilityDtoProfile : Profile
    {
        public RoomFacilityDtoProfile()
        {
            CreateMap<RoomFacility, RoomFacilityDto>()
                .ForMember(destination => destination.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<RoomFacilityDto, RoomFacility>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
