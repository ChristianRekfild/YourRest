using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class AccommodationDtoProfile : Profile
    {
        public AccommodationDtoProfile()
        {
            //CreateMap<AccommodationExtendedDto, Accommodation>()
            //    .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(target => target.Address, opt => opt.MapFrom(src => src.Address))
            //    .ForMember(target => target.AddressId, opt => opt.MapFrom(src => src.Address.Id))
            //    .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name))
            //    .ForMember(target => target.AccommodationType, opt => opt.MapFrom(src => src.AccommodationType))
            //    .ForMember(target => target.AccommodationTypeId, opt => opt.MapFrom(src => src.AccommodationType.Id))
            //    .ForMember(target => target.Rooms, opt => opt.MapFrom(src => src.Rooms))
            //    .ForMember(target => target.StarRating, opt => opt.MapFrom(src => new AccommodationStarRating() { Stars = (int)src.Stars }))
            //    .ReverseMap();

            CreateMap<Accommodation, AccommodationExtendedDto>()
                .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(target => target.Address, opt => opt.MapFrom(src => src.Address))
                .ForPath(target => target.Address.Id, opt => opt.MapFrom(src => src.AddressId))
                .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(target => target.AccommodationType, opt => opt.MapFrom(src => src.AccommodationType))
                .ForMember(target => target.Rooms, opt => opt.MapFrom(src => src.Rooms))
                .ForMember(target => target.Stars, opt => opt.MapFrom(src => src.StarRating.Stars))
                .ForPath(target => target.AccommodationType.Id, opt => opt.MapFrom(src => src.AccommodationTypeId))
                .ReverseMap();
        }
    }  
}
