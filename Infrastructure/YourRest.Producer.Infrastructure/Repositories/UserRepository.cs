using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;
using System.Linq.Expressions;

namespace YourRest.Producer.Infrastructure.Repositories;

public class UserRepository : PgRepository<User, int>, IUserRepository
{
    public UserRepository(SharedDbContext context) : base(context)
    {
        
    }
}