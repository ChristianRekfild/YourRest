using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class GuestRepository : PgRepository<Guest, int>, IGuestRepository
    {
        public GuestRepository(SharedDbContext dbContext) : base(dbContext) { }
    }
}
