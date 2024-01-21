using YourRest.Application.Dto.Models;
using YourRest.Application.Interfaces;
using YourRest.Infrastructure.Core.Contracts.Repositories;

namespace YourRest.Application.UseCases
{
    public class GetUserInfoUseCase : IGetUserInfoUseCase
    {
        private readonly ITokenRepository _tokenRepository;

        public GetUserInfoUseCase(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<UserDto> ExecuteAsync(string userKeyCloakId, CancellationToken cancellationToken)
        {
            var keycloakUser = await _tokenRepository.GetUser(userKeyCloakId, cancellationToken);

            var userDto = new UserDto
            {
                Username = keycloakUser.Username,
                LastName = keycloakUser.LastName,
                FirstName = keycloakUser.FirstName,
                Email = keycloakUser.Email
            };
            
            return userDto;
        }
    }
}