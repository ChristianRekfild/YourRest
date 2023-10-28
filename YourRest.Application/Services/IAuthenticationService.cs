using YourRest.Application.Dto;

namespace YourRest.Application.Services;
public interface IAuthenticationService
{
    Task<TokenDto> AuthenticateAsync(string username, string password);
}

