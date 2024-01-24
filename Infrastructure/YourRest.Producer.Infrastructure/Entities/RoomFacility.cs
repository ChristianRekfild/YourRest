namespace YourRest.Producer.Infrastructure.Entities
{
    public class RoomFacility: IntBaseEntity
    {
        public string Name { get; set; }
        public ICollection<Room> Rooms { get; set; }

        public RoomFacility() => Rooms = new List<Room>();
    }
}
