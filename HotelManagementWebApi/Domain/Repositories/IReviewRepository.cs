using HotelManagementWebApi.Domain.Entities.Review;

namespace HotelManagementWebApi.Domain.Repositories;

public interface IReviewRepository
{
    Task<Review> SaveReviewAsync(Review review);
}