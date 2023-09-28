namespace YourRest.Application.Dto
{
    public class ReviewDto
    {
        public int BookingId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}