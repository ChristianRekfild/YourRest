using YourRest.Application.Dto.Models;

namespace YourRest.Application.Interfaces
{
    public interface IGetUserInfoUseCase
    {
        Task<UserDto> ExecuteAsync(string userKeyCloakId, CancellationToken cancellationToken);
    }
}
