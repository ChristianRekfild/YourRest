using AutoMapper;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class RoomExtendedDtoProfile : Profile
    {
        public RoomExtendedDtoProfile()
        {
            CreateMap<Infrastructure.Core.Contracts.Models.RoomDto, RoomExtendedDto>()
                .ForMember(destination => destination.RoomFacilities, opt => opt.MapFrom(src => src.RoomFacilities))
                .ReverseMap();
            CreateMap<RoomWithIdDto, RoomExtendedDto>()
                .ForMember(destination => destination.RoomFacilities, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
