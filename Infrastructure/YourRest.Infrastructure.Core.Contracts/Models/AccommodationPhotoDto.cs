namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class AccommodationPhotoDto : IntBaseEntityDto
    {
        public string FilePath { get; set; }
        public AccommodationDto Accommodation { get; set; }
        public int AccommodationId { get; set; }
    }
}
