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
                cfg.CreateMap<RoomFacilityViewModel, RoomFacilityEntity>()
                .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(target => target.RoomId, opt => opt.MapFrom(src => src.RoomId))
                .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name));
            });
        private static MapperConfiguration ViewModelMap() => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RoomFacilityEntity, RoomFacilityViewModel>()
            .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(target => target.RoomId, opt => opt.MapFrom(src => src.RoomId))
            .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name));
        });

        public static RoomFacilityEntity ToEntity(this RoomFacilityViewModel source) =>
            new Mapper(EntityMap()).Map<RoomFacilityViewModel, RoomFacilityEntity>(source);
        public static IEnumerable<RoomFacilityEntity> ToEntity(this IEnumerable<RoomFacilityViewModel> source) =>
           new Mapper(EntityMap()).Map<IEnumerable<RoomFacilityViewModel>, IEnumerable<RoomFacilityEntity>>(source);

        public static RoomFacilityViewModel ToViewModel(this RoomFacilityEntity source) =>
            new Mapper(ViewModelMap()).Map<RoomFacilityEntity, RoomFacilityViewModel>(source);
        public static IEnumerable<RoomFacilityViewModel> ToViewModel(this IEnumerable<RoomFacilityEntity> source) =>
           new Mapper(ViewModelMap()).Map<IEnumerable<RoomFacilityEntity>, IEnumerable<RoomFacilityViewModel>>(source);
    }
}

