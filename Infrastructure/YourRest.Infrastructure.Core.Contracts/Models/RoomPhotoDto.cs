namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class RoomPhotoDto : IntBaseEntityDto
    {
        public string FilePath { get; set; }
        public RoomDto Room { get; set; }
        public int RoomId { get; set; }
    }
}
