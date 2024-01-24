namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public int RoomTypeId { get; set; }
        public string Name { get; set; }
        public double SquareInMeter { get; set; }
    }
}
