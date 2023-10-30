using AutoMapper;
using YourRest.Application.Dto.Models.Room;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class RoomWithIdDtoProfile: Profile
    {
        public RoomWithIdDtoProfile()
        {
            CreateMap<Room, RoomWithIdDto>()
                .ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
            CreateMap<RoomDto, RoomWithIdDto>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
