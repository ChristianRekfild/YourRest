using YourRest.Application.Dto;
using YourRest.Domain.Repositories;
using YourRest.Application.Exceptions;

namespace YourRest.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenRepository _tokenRepository;

    public AuthenticationService(ITokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }

    public async Task<TokenDto> AuthenticateAsync(string username, string password)
    {
        try
        {
            var result = await _tokenRepository.GetTokenAsync(username, password);
            return new TokenDto { AccessToken = result.access_token };
        }
        catch (Exception ex)
        {
            throw new ValidationException("Invalid credentials");
        }
    }
}
