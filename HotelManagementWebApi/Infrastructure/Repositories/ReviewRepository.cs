using HotelManagementWebApi.Domain.Repositories;
using HotelManagementWebApi.Infrastructure.Repositories.DbContexts;
using YourRest.Infrastructure.Repositories;
using HotelManagementWebApi.Domain.Entities;

namespace HotelManagementWebApi.Infrastructure.Repositories;

public class ReviewRepository : PgRepository<Review, int>, IReviewRepository
{
    public ReviewRepository(HotelManagementDbContext context) : base(context)
    {
    }
}