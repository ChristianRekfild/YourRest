using AutoMapper;
using YourRest.Application.Dto.Models.AccommodationFacility;

namespace YourRest.Application.Dto.Mappers.Profiles
{
    public class AccommodationFacilityWithIdProfile: Profile
    {
        public AccommodationFacilityWithIdProfile()
        {
            CreateMap <Infrastructure.Core.Contracts.Models.AccommodationFacilityDto, AccommodationFacilityWithIdDto>()
                //.ForMember(destination => destination.Id, opt => opt.MapFrom(src => src.Id))
                //.ForMember(destination => destination.Name, opt => opt.MapFrom(src => src.Name))
                //.ForMember(destination => destination.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap()
                .ForMember(destination => destination.AccommodationFacilities, opt => opt.Ignore());

            CreateMap<AccommodationFacilityDto, AccommodationFacilityWithIdDto>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
