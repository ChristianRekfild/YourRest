using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Repositories;

public class UserRepository : PgRepository<User, int, UserDto>, IUserRepository
{
    private readonly SharedDbContext _context;

    public UserRepository(SharedDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }
    public async Task<IEnumerable<UserDto>> GetUserWithPhotosAsync(string userKeyCloakId, CancellationToken cancellationToken = default)
    {
        return _mapper.Map<IEnumerable<UserDto>>(await _context.Users
            .Where(a => a.KeyCloakId == userKeyCloakId)
            .Include(a => a.UserPhotos)
            //.Select(user => new UserDto
            //{
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    Email = user.Email,
            //    KeyCloakId = user.KeyCloakId,
            //    UserAccommodations = user.UserAccommodations,
            //    UserPhotos = user.UserPhotos.OrderByDescending(photo => photo.Id).ToList()
            //})
            .ToListAsync(cancellationToken));
    }
}