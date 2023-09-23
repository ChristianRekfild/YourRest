using SharedKernel.Domain.Entities;

namespace HotelManagementWebApi.Application.UseCase.Reviews.Dto
{
    public class ReviewDto
    {
        public int BookingId { get; set; }
        public Rating Rating { get; set; }
        public string Comment { get; set; }
    }
}