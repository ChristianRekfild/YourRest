namespace YourRest.Application.Dto
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SquareInMeter { get; set; }

        public int AccomodationId { get; set; }
        public string RoomType { get; set; }
        
        public int Capacity { get; set; }

    }
}

