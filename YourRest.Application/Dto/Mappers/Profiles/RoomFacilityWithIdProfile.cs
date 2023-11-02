using AutoMapper;
using YourRest.Application.Dto.Models.RoomFacility;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class RoomFacilityWithIdProfile: Profile
    {
        public RoomFacilityWithIdProfile()
        {
            CreateMap<RoomFacility, RoomFacilityWithIdDto>()
                .ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
             .ReverseMap();
            CreateMap<RoomFacilityDto, RoomFacilityWithIdDto>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
