using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories;

public class AccommodationTypeRepository : PgRepository<AccommodationType, int>, IAccommodationTypeRepository
{
    public AccommodationTypeRepository(SharedDbContext dataContext) : base(dataContext)
    {
    }
}