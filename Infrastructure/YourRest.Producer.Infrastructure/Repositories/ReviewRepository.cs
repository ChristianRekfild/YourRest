using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using YourRest.Infrastructure.Core.DbContexts;
using YourRest.Infrastructure.Core.Repositories;

namespace YourRest.Producer.Infrastructure.Repositories 
{
    public class ReviewRepository : PgRepository<Review, int>, IReviewRepository
    {
        public ReviewRepository(SharedDbContext context) : base(context)
        {
        }
    }
}

