using Microsoft.Extensions.DependencyInjection;
using YourRest.Application.Interfaces;
using YourRest.Application.UseCases;

namespace YourRest.Application
{
    public static class ApplicationDependencyInjections
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IGetCountryListUseCase, GetCountryListUseCase>();
            services.AddScoped<IGetCityByIdUseCase, GetCityByIdUseCase>();
            services.AddScoped<IGetCityListUseCase, GetCityListUseCase>();
            services.AddScoped<IGetRegionListUseCase, GetRegionListUseCase>();
            services.AddScoped<ICreateReviewUseCase, CreateReviewUseCase>();
            services.AddScoped<IAddAddressToAccommodationUseCase, AddAddressToAccommodationUseCase>();
            services.AddScoped<IGetCityByRegionIdUseCase, GetCityByRegionIdUseCase>();
            services.AddScoped<IGetCityByCountryIdUseCase, GetCityByCountryIdUseCase>();
            services.AddScoped<IGetRoomListUseCase, GetRoomListUseCase>();
            services.AddScoped<ICreateRoomUseCase, CreateRoomUseCase>();
            services.AddScoped<IGetRoomTypeListUseCase, GetRoomTypeListUseCase>();

            return services;
        }
    }
}
