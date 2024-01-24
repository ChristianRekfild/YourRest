using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IAuthorizationService
    {
        Task<HttpResponseMessage> LoginAsync(AuthorizationModel login, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> RegistrationAsync(RegistrationModel registration, CancellationToken cancellationToken = default);
    }
}
