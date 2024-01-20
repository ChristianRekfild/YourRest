namespace YourRest.Producer.Infrastructure.Entities
{
    public class AccommodationStarRating : IntBaseEntity
    {
        public int Stars { get; set; } // Can be 0, 1, 2, 3, 4, or 5
        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
    }
}