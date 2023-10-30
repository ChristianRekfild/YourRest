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
            .ForMember(target => target.AccommodationId, opt => opt.MapFrom(src => src.AccommodationId))
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(target => target.RoomType, opt => opt.MapFrom(src => src.RoomType))
            .ForMember(target => target.Capacity, opt => opt.MapFrom(src => src.Capacity))
            .ForMember(target => target.SquareInMeter, opt => opt.MapFrom(src => src.SquareInMeter))
            .ReverseMap();
        }
    }
}
