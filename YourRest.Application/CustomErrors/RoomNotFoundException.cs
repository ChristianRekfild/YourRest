namespace YourRest.Application.CustomErrors
{
    public class RoomCondlictException : Exception
    {
        public RoomCondlictException(string message) : base(message)
        {
        }
    }
}