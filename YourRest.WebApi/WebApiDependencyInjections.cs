using System.Reflection;

namespace YourRest.WebApi
{
    public static class WebApiDependencyInjections
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
