namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class RoomDto : IntBaseEntityDto
    { 
        public string Name { get; set; }
        public double SquareInMeter { get; set; }
        public int RoomTypeId { get; set; }
        public RoomTypeDto RoomType { get; set; }
        public int Capacity { get; set; }
        public AccommodationDto Accommodation { get; set; }
        public int AccommodationId { get; set; }
        public ICollection<RoomFacilityDto> RoomFacilities { get; set; }
        public RoomDto()
        {
            RoomFacilities = new List<RoomFacilityDto>();
        }
        public ICollection<BookingDto> bookings { get; set; }
    }
}
