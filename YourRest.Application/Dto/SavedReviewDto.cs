namespace YourRest.Application.Dto
{
    public class SavedReviewDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}