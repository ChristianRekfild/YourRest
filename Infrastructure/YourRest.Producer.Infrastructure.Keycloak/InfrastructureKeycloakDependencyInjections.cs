using Microsoft.Extensions.DependencyInjection;
using YourRest.Domain.Repositories;
using YourRest.Producer.Infrastructure.Keycloak.Repositories;

namespace YourRest.Producer.Infrastructure.Keycloak
{
    public static class InfrastructureKeycloakDependencyInjections
    {
        public static IServiceCollection AddKeycloakInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ITokenRepository, TokenRepository>();

            return services;
        }
    }
}
