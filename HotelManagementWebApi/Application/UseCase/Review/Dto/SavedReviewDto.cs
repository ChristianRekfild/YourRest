namespace HotelManagementWebApi.Application.UseCase.Review.Dto
{
    public class SavedReviewDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
    }
}