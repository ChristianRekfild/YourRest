using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Data.Services.Abstractions
{
    public interface IAuthorizationService
    {
        Task<HttpResponseMessage> LoginAsync(AuthorizationViewModel login, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> RegistrationAsync(RegistrationViewModel registration, CancellationToken cancellationToken = default);
    }
}
