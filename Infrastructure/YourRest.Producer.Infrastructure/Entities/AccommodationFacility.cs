namespace YourRest.Producer.Infrastructure.Entities
{
    public class AccommodationFacility: IntBaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<AccommodationFacilityLink> AccommodationFacilities { get; set; } = new List<AccommodationFacilityLink>();
    }
}
