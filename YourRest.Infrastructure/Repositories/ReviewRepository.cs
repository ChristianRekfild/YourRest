using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using YourRest.Infrastructure.DbContexts;

namespace YourRest.Infrastructure.Repositories;

public class ReviewRepository : PgRepository<Review, int>, IReviewRepository
{
    public ReviewRepository(SharedDbContext context) : base(context)
    {
    }
}