using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Dto
{
    public class EditAccommodationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AccommodationTypeId { get; set; }

        public int? Stars { get; set; }
        public string Description { get; set; }
    }
}