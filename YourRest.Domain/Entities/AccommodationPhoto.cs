namespace YourRest.Domain.Entities
{
    public class AccommodationPhoto : IntBaseEntity
    {
        public string FilePath { get; set; }
        public Accommodation Accommodation { get; set; }
        public int AccommodationId { get; set; }
    }
}
