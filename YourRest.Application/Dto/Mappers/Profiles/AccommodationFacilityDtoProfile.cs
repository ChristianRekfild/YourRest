using AutoMapper;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class AccommodationFacilityDtoProfile : Profile
    {
        public AccommodationFacilityDtoProfile()
        {
            CreateMap<Infrastructure.Core.Contracts.Models.AccommodationFacilityDto, Models.AccommodationFacility.AccommodationFacilityDto>()
                .ForMember(destination => destination.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<Models.AccommodationFacility.AccommodationFacilityDto, Infrastructure.Core.Contracts.Models.AccommodationFacilityDto>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
