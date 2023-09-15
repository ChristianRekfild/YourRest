namespace HotelManagementWebApi.Application.UseCase.Review.Dto
{
    public class ReviewDto
    {
        public int BookingId { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
    }
}