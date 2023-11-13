namespace YourRest.Domain.Entities
{
    public class RoomPhoto : IntBaseEntity
    {
        public string FilePath { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
    }
}
