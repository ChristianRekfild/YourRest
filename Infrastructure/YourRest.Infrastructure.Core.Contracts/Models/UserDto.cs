namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class UserDto : IntBaseEntityDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string KeyCloakId { get; set; }
        
        public ICollection<UserAccommodationDto> UserAccommodations { get; set; } = new List<UserAccommodationDto>();
        
        public ICollection<UserPhotoDto> UserPhotos { get; set; } = new List<UserPhotoDto>();
    }
}
