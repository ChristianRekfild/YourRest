using YouRest.HotelierWebApp.Data.ViewModels.Interfaces;

namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public static class ViewModelsDependencyInjection
    {
        public static IServiceCollection AddHotelierViewModels(this IServiceCollection services)
        {
            services.AddScoped<IHotelViewModel, HotelViewModel>();
            return services;
        }
    }
}
