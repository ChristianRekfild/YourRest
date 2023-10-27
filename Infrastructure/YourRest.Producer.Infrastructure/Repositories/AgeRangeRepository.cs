using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories;

public class AgeRangeRepository : PgRepository<AgeRange, int>, IAgeRangeRepository
{
    public AgeRangeRepository(SharedDbContext dataContext) : base(dataContext)
    {
    }
}