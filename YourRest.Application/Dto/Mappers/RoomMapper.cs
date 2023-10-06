using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers
{
    public static class RoomMapper
    {
        private static MapperConfiguration EntityMap() => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RoomViewModel, Room>()
            .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(target => target.AccommodationId, opt => opt.MapFrom(src => src.AccommodationId))
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(target => target.RoomFacilities, opt => opt.MapFrom(src => src.RoomFacilities));

            cfg.CreateMap<RoomFacilityViewModel, RoomFacility>()
            .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(target => target.RoomId, opt => opt.MapFrom(src => src.RoomId))
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name));
        });
        private static MapperConfiguration ViewModelMap() => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Room, RoomViewModel>()
            .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(target => target.AccommodationId, opt => opt.MapFrom(src => src.AccommodationId))
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(target => target.RoomFacilities, opt => opt.MapFrom(src => src.RoomFacilities));

            cfg.CreateMap<RoomFacility, RoomFacilityViewModel>()
            .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(target => target.RoomId, opt => opt.MapFrom(src => src.RoomId))
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name));
        });

        public static Room ToEntity(this RoomViewModel source) =>
            new Mapper(EntityMap()).Map<RoomViewModel, Room>(source);
        public static IEnumerable<Room> ToEntity(this IEnumerable<RoomViewModel> source) =>
           new Mapper(EntityMap()).Map<IEnumerable<RoomViewModel>, IEnumerable<Room>>(source);

        public static RoomViewModel ToViewModel(this Room source) =>
            new Mapper(ViewModelMap()).Map<Room, RoomViewModel>(source);
        public static IEnumerable<RoomViewModel> ToViewModel(this IEnumerable<Room> source) =>
           new Mapper(ViewModelMap()).Map<IEnumerable<Room>, IEnumerable<RoomViewModel>>(source);
    }
}
