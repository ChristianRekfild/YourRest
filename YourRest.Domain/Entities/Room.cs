namespace YourRest.Domain.Entities
{
    public class Room : IntBaseEntity
    {
        public Room() => RoomFacilities = new List<RoomFacility>();
        public string Name { get; set; }

        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public ICollection<RoomFacility> RoomFacilities { get; set; }
    }
}
