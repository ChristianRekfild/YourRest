namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class RoomFacilityDto: IntBaseEntityDto
    {
        public string Name { get; set; }
        public ICollection<RoomDto> Rooms { get; set; }

        public RoomFacilityDto() => Rooms = new List<RoomDto>();
    }
}
