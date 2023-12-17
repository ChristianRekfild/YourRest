using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class UserPhotoRepository : PgRepository<UserPhoto, int>, IUserPhotoRepository
    {
        public UserPhotoRepository(SharedDbContext context) : base(context)
        {
        }
    }
}