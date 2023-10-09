namespace YourRest.Application.Dto
{
    public class RoomDto
    {
        public string Name { get; set; }
        public int SquareInMeter { get; set; }

        public int AccommodationId { get; set; }
        public string RoomType { get; set; }
        
        public int Capacity { get; set; }

    }
}

