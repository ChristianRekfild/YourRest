using HotelManagementWebApi.Domain.Repositories;
using SharedKernel.Domain.Entities;
using YourRest.Infrastructure.DbContexts;
using YourRest.Infrastructure.Repositories;

namespace HotelManagementWebApi.Infrastructure.Repositories;

public class ReviewRepository : PgRepository<Review, int>, IReviewRepository
{
    public ReviewRepository(SharedDbContext context) : base(context)
    {
    }
}