namespace YourRestDomain.Entities
{
    public class AdditionalRoomServiceEntity : BaseEntity<int>
    {
        public AdditionalRoomServiceEntity() { }
        public int RoomId { get; set; }
        public string ServiceName { get; set; }

    }
}
