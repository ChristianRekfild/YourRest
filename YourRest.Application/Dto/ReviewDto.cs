using YourRest.Domain.Entities;

namespace YourRest.Application.Dto
{
    public class ReviewDto
    {
        public int BookingId { get; set; }
        public Rating Rating { get; set; }
        public string Comment { get; set; }
    }
}