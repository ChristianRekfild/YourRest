using HotelManagementWebApi.Domain.Repositories;
using HotelManagementWebApi.Infrastructure.Repositories.DbContexts;
using HotelManagementWebApi.Domain.Entities.Review;

namespace HotelManagementWebApi.Infrastructure.Repositories;

public class ReviewRepository : PgRepository<Review, int>, IReviewRepository
{
    private readonly HotelManagementDbContext _context;

    public ReviewRepository(HotelManagementDbContext context) : base(context)
    {
        _context = context;
    }
}