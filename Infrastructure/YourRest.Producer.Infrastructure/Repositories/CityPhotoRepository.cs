using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class CityPhotoRepository : PgRepository<CityPhoto, int>, ICityPhotoRepository
    {
        public CityPhotoRepository(SharedDbContext context) : base(context)
        {
        }
    }
}