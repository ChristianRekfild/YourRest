using AutoMapper;
using HotelManagementWebApi.ViewModels;
using YourRestDomain.Entities;

namespace YorRestDataTransferObject
{
    public static class AdditionalRoomServiceModelServiceDTO
    {
        public static MapperConfiguration ToServiceModelMap() => new MapperConfiguration(cfg =>
        {

            cfg.CreateMap<AdditionalRoomServiceViewModel, AdditionalRoomServiceEntity>()
                .ForMember(src => src.ServiceName, opt => opt.MapFrom(target => target.ServiceName))
                .ForMember(src => src.Id, opt => opt.MapFrom(target => target.Id))
                .ForMember(src => src.RoomId, opt => opt.MapFrom(target => target.RoomId));

        });
        public static MapperConfiguration ToEntityMap() => new MapperConfiguration(cfg =>
        {

            cfg.CreateMap<AdditionalRoomServiceViewModel, AdditionalRoomServiceEntity>()
                .ForMember(src => src.ServiceName, opt => opt.MapFrom(target => target.ServiceName))
                .ForMember(src => src.Id, opt => opt.MapFrom(target => target.Id))
                .ForMember(src => src.RoomId, opt => opt.MapFrom(target => target.RoomId));
        });

        public static AdditionalRoomServiceEntity ToEntity(this AdditionalRoomServiceViewModel source) => new Mapper(ToEntityMap()).Map<AdditionalRoomServiceViewModel, AdditionalRoomServiceEntity>(source);
        public static IEnumerable<AdditionalRoomServiceEntity> ToEntity(this IEnumerable<AdditionalRoomServiceViewModel> source) => new Mapper(ToEntityMap())
            .Map<IEnumerable<AdditionalRoomServiceViewModel>, IEnumerable<AdditionalRoomServiceEntity>>(source);

        public static AdditionalRoomServiceViewModel ToModelService(this AdditionalRoomServiceEntity source) => new Mapper(ToServiceModelMap()).Map<AdditionalRoomServiceEntity, AdditionalRoomServiceViewModel>(source);
        public static IEnumerable<AdditionalRoomServiceViewModel> ToDomain(this IEnumerable<AdditionalRoomServiceEntity> source) => new Mapper(ToServiceModelMap())
            .Map<IEnumerable<AdditionalRoomServiceEntity>, IEnumerable<AdditionalRoomServiceViewModel>>(source);
    }
}
