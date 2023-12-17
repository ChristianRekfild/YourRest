using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Dto
{
    public class AccommodationExtendedDto: AccommodationDto
    {
        public AddressDto Address { get; set; }
        public List<RoomWithIdDto> Rooms { get; set; }
    }
}