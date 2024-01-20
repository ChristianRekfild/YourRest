using AutoMapper;
using YourRest.Infrastructure.Core.Contracts.Models;
using YourRest.Infrastructure.Core.Contracts.Repositories;
using YourRest.Producer.Infrastructure.DbContexts;
using YourRest.Producer.Infrastructure.Entities;

namespace YourRest.Producer.Infrastructure.Repositories
{
    public class ReviewRepository : PgRepository<Review, int, ReviewDto>, IReviewRepository
    {
        public ReviewRepository(SharedDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}

