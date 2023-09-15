using HotelManagementWebApi.Domain.Entities.Review;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ReviewEntity = HotelManagementWebApi.Domain.Entities.Review.Review;

namespace HotelManagementWebApi.Domain.Repositories;

public interface IReviewRepository : IPgRepository<ReviewEntity, int>
{

}