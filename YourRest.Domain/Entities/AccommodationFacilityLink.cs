namespace YourRest.Domain.Entities
{
    public class AccommodationFacilityLink : IntBaseEntity
    {
        public int AccommodationFacilityId { get; set; }
        public AccommodationFacility AccommodationFacility { get; set; }

        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
    }

}
