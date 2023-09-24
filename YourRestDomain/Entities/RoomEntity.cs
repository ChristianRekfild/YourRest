namespace YourRestDomain.Entities
{
    public class RoomEntity : BaseEntity<int>
    {
        public RoomEntity() => RoomServices = new List<AdditionalRoomServiceEntity>();

        public string Name { get; set; }
        public ICollection<AdditionalRoomServiceEntity> RoomServices { get; set; }
    }
}
