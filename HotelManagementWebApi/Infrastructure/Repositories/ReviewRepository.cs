using HotelManagementWebApi.Domain.Entities.Review;
using HotelManagementWebApi.Infrastructure.Repositories.DbContexts;

namespace HotelManagementWebApi.Infrastructure.Repositories;

public class ReviewRepository : PgRepository<Review, int>, IReviewRepository
{
    public ReviewRepository(HotelManagementDbContext context) : base(context)
    {
    }
}