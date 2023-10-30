namespace YourRest.Application.Dto.Models.Room
{
    public class RoomExtendedDto : RoomWithIdDto
    {
        public List<RoomFacilityDto> RoomFacilities { get; set; }
    }
}
