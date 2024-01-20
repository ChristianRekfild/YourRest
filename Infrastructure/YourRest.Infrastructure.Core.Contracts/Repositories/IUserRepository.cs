using YourRest.Infrastructure.Core.Contracts.Models;

namespace YourRest.Infrastructure.Core.Contracts.Repositories
{
    public interface IUserRepository : IRepository<UserDto, int>
    {
        Task<IEnumerable<UserDto>> GetUserWithPhotosAsync(string userKeyCloakId, CancellationToken cancellationToken = default);
    }
}