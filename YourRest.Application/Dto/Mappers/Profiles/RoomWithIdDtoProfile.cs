using AutoMapper;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class RoomWithIdDtoProfile: Profile
    {
        public RoomWithIdDtoProfile()
        {
            CreateMap<Infrastructure.Core.Contracts.Models.RoomDto, RoomWithIdDto>()
                .ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
            CreateMap<RoomDto, RoomWithIdDto>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
