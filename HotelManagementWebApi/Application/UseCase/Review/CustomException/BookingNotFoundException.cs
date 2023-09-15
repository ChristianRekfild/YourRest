namespace HotelManagementWebApi.Application.UseCase.Review.CustomException
{
    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException(string message) : base(message)
        {
        }
    }
}