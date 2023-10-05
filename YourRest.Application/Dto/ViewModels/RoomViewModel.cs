using YourRest.Domain.Entities;

namespace YourRest.Application.Dto.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public string Name { get; set; }
        public List<RoomFacilityEntity> RoomFacilities { get; set; }
    }
}
