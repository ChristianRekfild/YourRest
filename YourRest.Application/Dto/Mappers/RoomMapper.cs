using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.Room;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers
{
    public static class RoomMapper
    {
        private static MapperConfiguration EntityMap() => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RoomWithIdDto, Room>()
            .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(target => target.AccommodationId, opt => opt.MapFrom(src => src.AccommodationId))
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(target => target.RoomType, opt => opt.MapFrom(src => src.RoomType))
            .ForMember(target => target.Capacity, opt => opt.MapFrom(src => src.Capacity))
            .ForMember(target => target.SquareInMeter, opt => opt.MapFrom(src => src.SquareInMeter));
            //.ForMember(target => target.RoomFacilities, opt => opt.MapFrom(src => src.RoomFacilities));

            cfg.CreateMap<RoomFacilityDto, RoomFacility>()
            .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(target => target.RoomId, opt => opt.MapFrom(src => src.RoomId))
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name));
        });
        private static MapperConfiguration ViewModelMap() => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Room, RoomWithIdDto>()
            .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(target => target.AccommodationId, opt => opt.MapFrom(src => src.AccommodationId))
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(target => target.RoomType, opt => opt.MapFrom(src => src.RoomType))
            .ForMember(target => target.Capacity, opt => opt.MapFrom(src => src.Capacity))
            .ForMember(target => target.SquareInMeter, opt => opt.MapFrom(src => src.SquareInMeter));
            //.ForMember(target => target.RoomFacilities, opt => opt.MapFrom(src => src.RoomFacilities));

            cfg.CreateMap<RoomFacility, RoomFacilityDto>()
            .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(target => target.RoomId, opt => opt.MapFrom(src => src.RoomId))
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name));
        });

        public static Room ToEntity(this RoomWithIdDto source) =>
            new Mapper(EntityMap()).Map<RoomWithIdDto, Room>(source);
        public static IEnumerable<Room> ToEntity(this IEnumerable<RoomWithIdDto> source) =>
           new Mapper(EntityMap()).Map<IEnumerable<RoomWithIdDto>, IEnumerable<Room>>(source);

        public static RoomWithIdDto ToViewModel(this Room source) =>
            new Mapper(ViewModelMap()).Map<Room, RoomWithIdDto>(source);
        public static IEnumerable<RoomWithIdDto> ToViewModel(this IEnumerable<Room> source) =>
           new Mapper(ViewModelMap()).Map<IEnumerable<Room>, IEnumerable<RoomWithIdDto>>(source);
    }
}
