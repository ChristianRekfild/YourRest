using AutoMapper;
using YourRest.Application.Dto.Models;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class AddressDtoProfile : Profile
    {
        public AddressDtoProfile()
        {
            CreateMap<AddressWithIdDto, Address>()
                .ForMember(target => target.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(target => target.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(target => target.ZipCode, opt => opt.MapFrom(src => src.ZipCode))
                .ForMember(target => target.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(target => target.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(target => target.CityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(target => target.City, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
