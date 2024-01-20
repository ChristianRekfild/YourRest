using AutoMapper;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class RoomPhotoRepository : PgRepository<RoomPhoto, int, RoomPhotoDto>, IRoomPhotoRepository
    {
        public RoomPhotoRepository(SharedDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}