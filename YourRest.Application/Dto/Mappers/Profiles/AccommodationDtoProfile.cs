using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class AccommodationDtoProfile : Profile
    {
        public AccommodationDtoProfile()
        {
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
