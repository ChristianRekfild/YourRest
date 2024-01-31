namespace YouRest.HotelierWebApp.Data.Models
{
    public class FormHotelModel
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string HotelName { get; set; }
        public string HotelType { get; set; }
        public string HotelRating { get; set; }
        public string HotelDescription { get; set; }
        public string ZipCode { get; set; }
        public List<HotelImgModel>? ImagesForLoad { get; set; }
        public List<string>? ImagesForView { get; set; }
    }
}
