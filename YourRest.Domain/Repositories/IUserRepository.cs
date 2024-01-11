using YourRest.Domain.Entities;

namespace YourRest.Domain.Repositories;
public interface IUserRepository : IRepository<User, int>
{
    Task<IEnumerable<User>> GetUserWithPhotosAsync(string userKeyCloakId, CancellationToken cancellationToken = default);
}