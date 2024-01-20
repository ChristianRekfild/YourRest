namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class UserPhotoDto : IntBaseEntityDto
    {
        public string FilePath { get; set; }
        public UserDto User { get; set; }
        public int UserId { get; set; }
    }
}
