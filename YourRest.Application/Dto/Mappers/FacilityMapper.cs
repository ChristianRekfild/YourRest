using AutoMapper;
using System.Security.Principal;
using YourRest.Application.Dto.Models;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers
{
    public static class FacilityMapper
    {
        private static MapperConfiguration EntityMap() => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoomFacilityDto, RoomFacility>()
                .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(target => target.RoomId, opt => opt.MapFrom(src => src.RoomId))
                .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name));
            });
        private static MapperConfiguration ViewModelMap() => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RoomFacility, RoomFacilityDto>()
            .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(target => target.RoomId, opt => opt.MapFrom(src => src.RoomId))
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name));
        });

        public static RoomFacility ToEntity(this RoomFacilityDto source) =>
            new Mapper(EntityMap()).Map<RoomFacilityDto, RoomFacility>(source);
        public static IEnumerable<RoomFacility> ToEntity(this IEnumerable<RoomFacilityDto> source) =>
           new Mapper(EntityMap()).Map<IEnumerable<RoomFacilityDto>, IEnumerable<RoomFacility>>(source);

        public static RoomFacilityDto ToViewModel(this RoomFacility source) =>
            new Mapper(ViewModelMap()).Map<RoomFacility, RoomFacilityDto>(source);
        public static IEnumerable<RoomFacilityDto> ToViewModel(this IEnumerable<RoomFacility> source) =>
           new Mapper(ViewModelMap()).Map<IEnumerable<RoomFacility>, IEnumerable<RoomFacilityDto>>(source);
    }
}

