using AutoMapper;
using YourRest.Application.Dto.Models.AccommodationFacility;
using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class AccommodationFacilityWithIdProfile: Profile
    {
        public AccommodationFacilityWithIdProfile()
        {
            CreateMap<AccommodationFacility, AccommodationFacilityWithIdDto>()
                .ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
             .ReverseMap();
            CreateMap<AccommodationFacilityDto, AccommodationFacilityWithIdDto>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
