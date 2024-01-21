using AutoMapper;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class RoomFacilityDtoProfile : Profile
    {
        public RoomFacilityDtoProfile()
        {
            CreateMap<RoomFacility, Models.RoomFacility.RoomFacilityDto>()
                .ForMember(destination => destination.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<Models.RoomFacility.RoomFacilityDto, Infrastructure.Core.Contracts.Models.RoomFacilityDto>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
