
namespace YourRest.Application.CustomErrors
{
    public class RoomNotFoundExeption : Exception
    {
        public RoomNotFoundExeption(int roomId) :
            base($"Room with id number {roomId} not found")
        {
        }

    }
}
