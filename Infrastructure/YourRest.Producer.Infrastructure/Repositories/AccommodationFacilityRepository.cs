using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class AccommodationFacilityRepository : PgRepository<AccommodationFacility, int>, IAccommodationFacilityRepository
    {
        public AccommodationFacilityRepository(SharedDbContext context) : base(context)
        {
        }
    }
}
