using Microsoft.Extensions.DependencyInjection;
using YourRest.Application.Interfaces;
using YourRest.Application.Interfaces.Facility;
using YourRest.Application.Interfaces.Room;
using YourRest.Application.UseCases;
using YourRest.Application.UseCases.Facility;
using YourRest.Application.UseCases.Room;

namespace YourRest.Application
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) 
        {
            services.AddScoped<IGetCountryListUseCase, GetCountryListUseCase>();
            services.AddScoped<IGetCityByIdUseCase, GetCityByIdUseCase>();
            services.AddScoped<IGetCityListUseCase, GetCityListUseCase>();
            services.AddScoped<IGetRegionListUseCase, GetRegionListUseCase>();
            services.AddScoped<ICreateReviewUseCase, CreateReviewUseCase>();
            services.AddScoped<IAddAddressToAccommodationUseCase, AddAddressToAccommodationUseCase>();
            //Room
            services.AddScoped<IAddRoomUseCase, AddRoomUseCase>();
            services.AddScoped<IEditRoomUseCase, EditRoomUseCase>();
            services.AddScoped<IGetRoomByIdUseCase, GetRoomByIdUseCase>();
            services.AddScoped<IRemoveRoomUseCase, RemoveRoomUseCase>();
            //RoomFacility
            services.AddScoped<IAddRoomFacilityUseCase, AddRoomFacilityUseCase>();
            services.AddScoped<IEditRoomFacilityUseCase, EditRoomFacilityUseCase>();
            services.AddScoped<IGetRoomFacilitiesByRoomIdUseCase, GetRoomFacilitiesByRoomIdUseCase>();
            services.AddScoped<IGetRoomFacilityByIdUseCase, GetRoomFacilityByIdUseCase>();
            services.AddScoped<IRemoveRoomFacilityUseCase, RemoveRoomFacilityUseCase>();

            return services;
        }
    }
}
