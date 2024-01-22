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
            CreateMap<Accommodation, AccommodationDto>()
                .ReverseMap()/*
                .ForMember(dest => dest.AccommodationFacilities, opt => opt.Ignore())
                .ForMember(dest => dest.AccommodationType, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .ForMember(dest => dest.Rooms, opt => opt.Ignore())
                .ForMember(dest => dest.StarRating, opt => opt.Ignore())*/;

            CreateMap<AccommodationFacility, AccommodationFacilityDto>()
                .ReverseMap()
                .ForMember(dest => dest.AccommodationFacilities, opt => opt.Ignore());

            CreateMap<AccommodationFacilityLinkDto, AccommodationFacilityLink>()
                .ReverseMap()
                .ForMember(dest => dest.Accommodation, opt => opt.Ignore())
                .ForMember(dest => dest.AccommodationFacility, opt => opt.Ignore());

            CreateMap<AccommodationPhoto, AccommodationPhotoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Accommodation, opt => opt.Ignore());

            CreateMap<AccommodationStarRating, AccommodationStarRatingDto>()
                .ReverseMap()
                .ForMember(dest => dest.Accommodation, opt => opt.Ignore());

            CreateMap<AccommodationTypeDto, AccommodationType>()
               .ReverseMap();

            CreateMap<AgeRangeDto, AgeRange>()
               .ReverseMap();

            CreateMap<Address, AddressDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

                //.ForMember(dest => dest.City, opt => opt.Ignore())
                //.ReverseMap()
                //.ForMember(dest => dest.City, opt => opt.Ignore());

            CreateMap<Booking, BookingDto>()
                 .ReverseMap();

            CreateMap<BookingStatusDto, BookingStatus>()
                 .ReverseMap();

            CreateMap<City, CityDto>()
                .ReverseMap()
                .ForMember(dest => dest.Addresses, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore());

            CreateMap<Country, CountryDto>()
                .ReverseMap()/*
                .ForMember(dest => dest.Regions, opt => opt.Ignore())*/;

            CreateMap<CustomerDto, Customer>()
                .ReverseMap();

            CreateMap<Region, RegionDto>()
                .ReverseMap()
                .ForMember(dest => dest.Cities, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore());

            CreateMap<Review, ReviewDto>()
                .ReverseMap()
                .ForMember(dest => dest.Booking, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Room, RoomDto>()
                .ReverseMap()/*
                .ForMember(dest => dest.Accommodation, opt => opt.Ignore())
                .ForMember(dest => dest.RoomType, opt => opt.Ignore())
                .ForMember(dest => dest.RoomFacilities, opt => opt.Ignore())*/;

            CreateMap<RoomFacility, RoomFacilityDto>()
                .ReverseMap()
                .ForMember(dest => dest.Rooms, opt => opt.Ignore());

            CreateMap<RoomPhoto, RoomPhotoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Room, opt => opt.Ignore());

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
                .ReverseMap()
                .ForMember(dest => dest.UserAccommodations, opt => opt.Ignore())
                .ForMember(dest => dest.UserPhotos, opt => opt.Ignore());

            CreateMap<UserAccommodation, UserAccommodationDto>()
                .ReverseMap()
                .ForMember(dest => dest.Accommodation, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UserPhotoDto, UserPhoto>()
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            //configuration.AssertConfigurationIsValid();
        }
    }
}
