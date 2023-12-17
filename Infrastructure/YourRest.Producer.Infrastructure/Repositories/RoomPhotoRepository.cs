using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class RoomPhotoRepository : PgRepository<RoomPhoto, int>, IRoomPhotoRepository
    {
        public RoomPhotoRepository(SharedDbContext context) : base(context)
        {
        }
    }
}