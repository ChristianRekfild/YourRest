using YourRest.Application.Dto.Models;

namespace YourRest.Application.CustomErrors
{
    public class RoomFacilityInProcessException: Exception
    {
        public RoomFacilityInProcessException(RoomFacilityViewModel roomFacility) :
            base($"Room Facility \"{roomFacility.Name}\" has been in process")
        {
        }
    }
}
