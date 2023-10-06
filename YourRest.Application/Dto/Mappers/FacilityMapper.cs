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
                cfg.CreateMap<RoomFacilityViewModel, RoomFacility>()
                .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(target => target.RoomId, opt => opt.MapFrom(src => src.RoomId))
                .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name));
            });
        private static MapperConfiguration ViewModelMap() => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RoomFacility, RoomFacilityViewModel>()
            .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(target => target.RoomId, opt => opt.MapFrom(src => src.RoomId))
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name));
        });

        public static RoomFacility ToEntity(this RoomFacilityViewModel source) =>
            new Mapper(EntityMap()).Map<RoomFacilityViewModel, RoomFacility>(source);
        public static IEnumerable<RoomFacility> ToEntity(this IEnumerable<RoomFacilityViewModel> source) =>
           new Mapper(EntityMap()).Map<IEnumerable<RoomFacilityViewModel>, IEnumerable<RoomFacility>>(source);

        public static RoomFacilityViewModel ToViewModel(this RoomFacility source) =>
            new Mapper(ViewModelMap()).Map<RoomFacility, RoomFacilityViewModel>(source);
        public static IEnumerable<RoomFacilityViewModel> ToViewModel(this IEnumerable<RoomFacility> source) =>
           new Mapper(ViewModelMap()).Map<IEnumerable<RoomFacility>, IEnumerable<RoomFacilityViewModel>>(source);
    }
}

