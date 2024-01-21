using AutoMapper;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class RoomFacilityDtoProfile : Profile
    {
        public RoomFacilityDtoProfile()
        {
            CreateMap<Infrastructure.Core.Contracts.Models.RoomFacilityDto, Models.RoomFacility.RoomFacilityDto>()
                .ForMember(destination => destination.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<Models.RoomFacility.RoomFacilityDto, Infrastructure.Core.Contracts.Models.RoomFacilityDto>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
