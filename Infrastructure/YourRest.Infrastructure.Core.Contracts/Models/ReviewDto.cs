using YourRest.Infrastructure.Core.Contracts.ValueObjects.Reviews;

namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class ReviewDto : IntBaseEntityDto
    {
        public BookingDto Booking { get; set; }
        public UserDto User { get; set; }
        public string Comment { get; set; }
        public RatingVO Rating { get; set; }

        public int BookingId { get; set; }
        public int UserId { get; set; }
    }
}
