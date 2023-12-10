using YourRest.Application.Exceptions;
using YourRest.Application.Interfaces;
using YourRest.Domain.Entities;
using YourRest.Domain.ValueObjects.Reviews;
using YourRest.Domain.Repositories;
using YourRest.Application.Dto.Models;

namespace YourRest.Application.UseCases
{
    public class GetUserInfoUseCase : IGetUserInfoUseCase
    {
        private readonly ITokenRepository _tokenRepository;

        public GetUserInfoUseCase(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<UserDto> Execute(string userKeyCloakId)
        {
            var keycloakUser = await _tokenRepository.GetUser(userKeyCloakId);

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