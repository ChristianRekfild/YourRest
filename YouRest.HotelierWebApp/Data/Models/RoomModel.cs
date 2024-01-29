namespace YouRest.HotelierWebApp.Data.Models
{
    public class RoomModel
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public int RoomTypeId { get; set; }
        public string Name { get; set; }
        public double SquareInMeter { get; set; }
    }
}
