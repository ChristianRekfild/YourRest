using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace YourRest.Producer.Infrastructure.Repositories;

public class UserRepository : PgRepository<User, int>, IUserRepository
{
    private readonly SharedDbContext _context;

    public UserRepository(SharedDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<IEnumerable<User>> GetUserWithPhotosAsync(string userKeyCloakId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(a => a.KeyCloakId == userKeyCloakId)
            .Include(a => a.UserPhotos)
            .Select(user => new User 
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                KeyCloakId = user.KeyCloakId,
                UserAccommodations = user.UserAccommodations,
                UserPhotos = user.UserPhotos.OrderByDescending(photo => photo.Id).ToList()
            })
            .ToListAsync(cancellationToken);
    }
}