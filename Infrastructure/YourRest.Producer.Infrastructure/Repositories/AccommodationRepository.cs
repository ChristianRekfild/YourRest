using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;
using System.Linq.Expressions;

namespace YourRest.Producer.Infrastructure.Repositories;

public class AccommodationRepository : PgRepository<Accommodation, int>, IAccommodationRepository
{
    public AccommodationRepository(SharedDbContext context) : base(context)
    {
        
    }
}