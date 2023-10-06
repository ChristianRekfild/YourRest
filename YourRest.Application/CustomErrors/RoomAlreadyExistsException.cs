using YourRest.Application.Dto.Models;

namespace YourRest.Application.CustomErrors
{
    public class RoomAlreadyExistsException: Exception
    {
        public RoomAlreadyExistsException(RoomViewModel room) :
            base($"Room with number {room.Id} is alredy exists")
        {
        }
        
    }
}
