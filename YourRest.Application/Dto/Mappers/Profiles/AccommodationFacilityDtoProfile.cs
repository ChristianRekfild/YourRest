using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using YourRest.Application.Dto.Models.AccommodationFacility;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class AccommodationFacilityDtoProfile : Profile
    {
        public AccommodationFacilityDtoProfile()
        {
            CreateMap<AccommodationFacility, AccommodationFacilityDto>()
                .ForMember(destination => destination.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<AccommodationFacilityDto, AccommodationFacility>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
