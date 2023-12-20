using YourRest.Application.Dto.Models;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Dto
{
    public class AccommodationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AccommodationTypeDto AccommodationType { get; set; }
        public string? Description { get; set; }
        
        public int? Stars { get; set; }
    }
}