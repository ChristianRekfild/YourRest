namespace YourRest.Application.CustomErrors
{
    public class RoomNotFoundException : Exception
    {
        public RoomNotFoundException(string message) : base(message)
        {
        }
    }
}