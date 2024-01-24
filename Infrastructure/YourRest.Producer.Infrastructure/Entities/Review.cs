using YourRest.Infrastructure.Core.Contracts.ValueObjects.Reviews;

namespace YourRest.Producer.Infrastructure.Entities
{
    public class Review : IntBaseEntity
    {
        public Booking Booking { get; set; }
        public User User { get; set; }
        public string Comment { get; set; }
        public RatingVO Rating { get; set; }

        public int BookingId { get; set; }
        public int UserId { get; set; }
    }
}
