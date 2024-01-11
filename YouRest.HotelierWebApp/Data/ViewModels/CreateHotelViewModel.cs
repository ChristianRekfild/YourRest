namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class CreateHotelViewModel
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string HotelName { get; set; }
        public string HotelType { get; set; }
        public string HotelRating { get; set; } = "Без рейтинга";
        public string HotelDescription { get; set; }
        public List<string> Images { get; set; } = new();
    }
}
