using YouRest.HotelierWebApp.Data.Services.Abstractions;

namespace YouRest.HotelierWebApp.Data.Services
{
    public static class HotelierWebAppDependencyInjection
    {
        public static IServiceCollection AddHotelierWebAppServices(this IServiceCollection services) 
        {
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IHotelTypeService, HotelTypeService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            return services;
        }
    }
}
