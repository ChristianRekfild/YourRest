namespace YourRest.Application.CustomErrors
{
    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException(string message) : base(message)
        {
        }
    }
}