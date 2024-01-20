namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class AccommodationFacilityDto: IntBaseEntityDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<AccommodationFacilityLinkDto> AccommodationFacilities { get; set; } = new List<AccommodationFacilityLinkDto>();
    }
}
