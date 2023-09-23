namespace HotelManagementWebApi.Application.UseCase.Reviews.CustomException
{
    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException(string message) : base(message)
        {
        }
    }
}