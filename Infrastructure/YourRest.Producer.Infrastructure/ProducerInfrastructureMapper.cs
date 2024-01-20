using AutoMapper;
using Microsoft.Extensions.Configuration;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure
{
    public class ProducerInfrastructureMapper : Profile
    {
        public ProducerInfrastructureMapper()
        {
            CreateMap<AccommodationDto, Accommodation>()
                .ReverseMap();
            
            CreateMap<AccommodationFacilityDto, AccommodationFacility>()
                .ReverseMap();

            CreateMap<AccommodationFacilityLinkDto, AccommodationFacilityLink>()
                .ReverseMap();

            CreateMap<AccommodationPhotoDto, AccommodationPhoto>()
                .ReverseMap();

            CreateMap<AccommodationStarRatingDto, AccommodationStarRating>()
                .ReverseMap();

            CreateMap<AccommodationTypeDto, Accommodation>()
               .ReverseMap();

            CreateMap<AgeRangeDto, AgeRange>()
               .ReverseMap();

            CreateMap<AddressDto, Address>()
                .ReverseMap();

            CreateMap<BookingDto, Booking>()
                 .ReverseMap();

            CreateMap<BookingStatusDto, BookingStatus>()
                 .ReverseMap();

            CreateMap<CityDto, City>()
                .ReverseMap();

            CreateMap<CountryDto, Country>()
                .ReverseMap();

            CreateMap<CustomerDto, Customer>()
                .ReverseMap();

            CreateMap<RegionDto, Region>()
                .ReverseMap();

            CreateMap<ReviewDto, Review>()
                .ReverseMap();

            CreateMap<RoomDto, Room>()
                .ReverseMap();

            CreateMap<RoomFacilityDto, RoomFacility>()
                .ReverseMap();

            CreateMap<RoomPhotoDto, RoomPhoto>()
                .ReverseMap();

            CreateMap<RoomTypeDto, RoomType>()
                .ReverseMap();
                        
            // TODO: Добавить сортировку
            /*
              UserPhotos = user.UserPhotos.OrderByDescending(photo => photo.Id).ToList()
             */
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.KeyCloakId, opt => opt.MapFrom(src => src.KeyCloakId))
                .ForMember(dest => dest.UserAccommodations, opt => opt.MapFrom(src => src.UserAccommodations))
                .ForMember(dest => dest.UserPhotos, opt => opt.MapFrom(src => src.UserPhotos))
                .ReverseMap();

            CreateMap<UserAccommodationDto, UserAccommodation>()
                .ReverseMap();

            CreateMap<UserPhotoDto, UserPhoto>()
                .ReverseMap();

            //configuration.AssertConfigurationIsValid();
        }
    }
}
