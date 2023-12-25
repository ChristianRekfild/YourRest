using AutoMapper;
using YourRest.Application.Dto.Models.Room;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class RoomDtoProfile: Profile
    {
        public RoomDtoProfile()
        {
            CreateMap<Room, RoomDto>()
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(target => target.RoomTypeId, opt => opt.MapFrom(src => src.RoomType.Id))
            .ForMember(target => target.Capacity, opt => opt.MapFrom(src => src.Capacity))
            .ForMember(target => target.SquareInMeter, opt => opt.MapFrom(src => src.SquareInMeter))
            .ReverseMap();
        }
    }
}
