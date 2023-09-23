using HotelManagementWebApi.Domain.Entities;
using SharedKernel.Domain.Repositories;

namespace HotelManagementWebApi.Domain.Repositories;

public interface IReviewRepository : IRepository<Review, int>
{

}