using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Dto
{
    public class CreateAccommodationDto
    {
        public string Name { get; set; }
        public int AccommodationTypeId { get; set; }
        public string Description { get; set; }
    }
}