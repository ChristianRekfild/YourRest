namespace YourRest.Domain.Entities
{
    public class Room : IntBaseEntity
    {
        public string Name { get; set; }
        public double SquareInMeter { get; set; }
        public string RoomType { get; set; }
        public int Capacity { get; set; }
        public Accommodation Accommodation { get; set; }
        public int AccommodationId { get; set; }
    }
}
