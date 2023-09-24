using AutoMapper;
using HotelManagementWebApi.ViewModels;
using YourRestDomain.Entities;

namespace YorRestDataTransferObject
{
    public static class RoomModelServiceDTO
    {
        public static MapperConfiguration ToServiceModelMap() => new MapperConfiguration(cfg =>
        {

            cfg.CreateMap<AdditionalRoomServiceViewModel, AdditionalRoomServiceEntity>()
                .ForMember(src => src.ServiceName, opt => opt.MapFrom(target => target.ServiceName))
                .ForMember(src => src.Id, opt => opt.MapFrom(target => target.Id));

        });
        public static MapperConfiguration ToEntityMap() => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<RoomEntity, RoomViewModel>()

               .ForMember(src => src.RoomName, opt => opt.MapFrom(target => target.Name))
               .ForMember(src => src.AdditionalRoomServices, opt => opt.MapFrom(target => target.RoomServices))
               .ForMember(src => src.Id, opt => opt.MapFrom(target => target.Id));

            cfg.CreateMap<AdditionalRoomServiceViewModel, AdditionalRoomServiceEntity>()
                .ForMember(src => src.ServiceName, opt => opt.MapFrom(target => target.ServiceName))
                .ForMember(src => src.Id, opt => opt.MapFrom(target => target.Id));
        });

        public static RoomEntity ToEntity(this RoomViewModel source) => new Mapper(ToEntityMap()).Map<RoomViewModel, RoomEntity>(source);
        public static IEnumerable<RoomEntity> ToEntity(this IEnumerable<RoomViewModel> source) => new Mapper(ToEntityMap())
            .Map<IEnumerable<RoomViewModel>, IEnumerable<RoomEntity>>(source);

        public static RoomViewModel ToModelService(this RoomEntity source) => new Mapper(ToServiceModelMap()).Map<RoomEntity, RoomViewModel>(source);
        public static IEnumerable<RoomViewModel> ToDomain(this IEnumerable<RoomEntity> source) => new Mapper(ToServiceModelMap())
            .Map<IEnumerable<RoomEntity>, IEnumerable<RoomViewModel>>(source);
    }
}
