namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class AccommodationStarRatingDto : IntBaseEntityDto
    {
        public int Stars { get; set; } // Can be 0, 1, 2, 3, 4, or 5
        public int AccommodationId { get; set; }
        public AccommodationDto Accommodation { get; set; }
    }
}