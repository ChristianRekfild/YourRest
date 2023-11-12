using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class AccommodationPhotoRepository : PgRepository<AccommodationPhoto, int>, IAccommodationPhotoRepository
    {
        public AccommodationPhotoRepository(SharedDbContext context) : base(context)
        {
        }
    }
}