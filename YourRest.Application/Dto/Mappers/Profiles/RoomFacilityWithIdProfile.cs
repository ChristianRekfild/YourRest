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
                .ForMember(destination => destination.Name, opt => opt.MapFrom(src => src.Name))                
                .ReverseMap()
                .ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(destination => destination.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(destination => destination.Rooms, opt => opt.Ignore());

            CreateMap<RoomFacilityDto, RoomFacilityWithIdDto>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
