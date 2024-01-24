namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class UserAccommodationDto : IntBaseEntityDto
    {
        public int UserId { get; set; }
        public UserDto User { get; set; }

        public int AccommodationId { get; set; }
        public AccommodationDto Accommodation { get; set; }
    }

}
