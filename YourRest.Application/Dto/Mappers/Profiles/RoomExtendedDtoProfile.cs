using AutoMapper;
using YourRest.Application.Dto.Models.Room;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class RoomExtendedDtoProfile : Profile
    {
        public RoomExtendedDtoProfile()
        {
            CreateMap<Room, RoomExtendedDto>()
                .ForMember(destination => destination.RoomFacilities, opt => opt.MapFrom(src => src.RoomFacilities))
                .ReverseMap();
            CreateMap<RoomWithIdDto, RoomExtendedDto>()
                .ForMember(destination => destination.RoomFacilities, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
