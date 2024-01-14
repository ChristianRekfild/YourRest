using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;

namespace YourRest.Application.Services;
public interface IAuthenticationService
{
    Task<TokenDto> AuthenticateAsync(string username, string password);

    Task<TokenDto> RegisterAsync(ExtendedUserCredentialsViewModel userDto, CancellationToken cancellationToken);
}

