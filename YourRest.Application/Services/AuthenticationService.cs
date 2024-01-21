using System.ComponentModel.DataAnnotations;
using YourRest.Application.Dto;
using YourRest.Application.Dto.ViewModels;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using ValidationException = YourRest.Application.Exceptions.ValidationException;

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
            throw new ValidationException($"Invalid credentials ({ex.Message})");
        }
    }
    
    public async Task<TokenDto> RegisterAsync(ExtendedUserCredentialsViewModel userDto, CancellationToken cancellationToken)
    {
        try
        {
            var token = await _tokenRepository.GetAdminTokenAsync(cancellationToken);
            var result = await _tokenRepository.CreateUser(
                token.access_token,
                userDto.Username,
                userDto.Firstname,
                userDto.Lastname,
                userDto.Email,
                userDto.Password
            );

            if (result == "")
            {
                throw new ValidationException($"User not created ({userDto.Username})");
            }

            var accessToken = await _tokenRepository.GetTokenAsync(userDto.Username, userDto.Password);
            
            return new TokenDto { AccessToken = accessToken.access_token };
        }
        catch (Exception ex)
        {
            throw new ValidationException($"Invalid credentials ({ex.Message})");
        }
    }
}
