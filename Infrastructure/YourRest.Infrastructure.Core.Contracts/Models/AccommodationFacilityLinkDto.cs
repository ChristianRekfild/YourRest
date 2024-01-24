namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class AccommodationFacilityLinkDto : IntBaseEntityDto
    {
        public int AccommodationFacilityId { get; set; }
        public AccommodationFacilityDto AccommodationFacility { get; set; }

        public int AccommodationId { get; set; }
        public AccommodationDto Accommodation { get; set; }
    }

}
