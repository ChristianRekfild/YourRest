using Microsoft.Extensions.DependencyInjection;
using YourRest.Domain.Repositories;
using YourRest.Producer.Infrastructure.Repositories;

namespace YourRest.Producer.Infrastructure
{
    public static class ProducerInfrastructureDependencyInjections
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IAccommodationRepository, AccommodationRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomFacilityRepository, RoomFacilityRepository>();     
            services.AddScoped<IAccommodationFacilityRepository, AccommodationFacilityRepository>();
            services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            services.AddScoped<IAgeRangeRepository, AgeRangeRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccommodationPhotoRepository, AccommodationPhotoRepository>();
            services.AddScoped<IRoomPhotoRepository, RoomPhotoRepository>();
            services.AddScoped<IAccommodationTypeRepository, AccommodationTypeRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IUserPhotoRepository, UserPhotoRepository>();
            services.AddScoped<ICityPhotoRepository, CityPhotoRepository>();

            return services;
        }
    }
}
