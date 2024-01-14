using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class AccommodationDtoProfile : Profile
    {
        public AccommodationDtoProfile()
        {
            CreateMap<AccommodationExtendedDto, Accommodation>()
                .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(target => target.AddressId, opt => opt.MapFrom(src => src.Address.Id))
                .ForMember(target => target.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(target => target.AccommodationType, opt => opt.MapFrom(src => src.AccommodationType))
                .ForMember(target => target.Rooms, opt => opt.MapFrom(src => src.Rooms))
                .ForMember(target => target.StarRating.Stars, opt => opt.MapFrom(src => src.Stars))
                .ReverseMap();
        }
    }
}
