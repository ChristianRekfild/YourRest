using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories;

public class AccommodationStarRatingRepository : PgRepository<AccommodationStarRating, int>, IAccommodationStarRatingRepository
{
    public AccommodationStarRatingRepository(SharedDbContext dataContext) : base(dataContext)
    {
    }
}