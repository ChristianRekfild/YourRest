namespace YourRest.Domain.Entities
{
    public class Room : IntBaseEntity
    {
        public Room() => RoomFacilities = new List<RoomFacilityEntity>();
        public string Name { get; set; }
        public ICollection<RoomFacilityEntity> RoomFacilities { get; set; }
    }
}
