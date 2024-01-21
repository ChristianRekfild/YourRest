using AutoMapper;
using YourRest.Application.Dto.Models.RoomFacility;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class RoomFacilityWithIdProfile: Profile
    {
        public RoomFacilityWithIdProfile()
        {
            CreateMap<Infrastructure.Core.Contracts.Models.RoomFacilityDto, RoomFacilityWithIdDto>()
                .ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
             .ReverseMap();
            CreateMap<RoomFacilityDto, RoomFacilityWithIdDto>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
