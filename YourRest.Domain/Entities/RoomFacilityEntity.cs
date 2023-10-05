namespace YourRest.Domain.Entities
{
    public class RoomFacilityEntity: IntBaseEntity
    {
        public string Name { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
