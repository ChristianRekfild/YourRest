namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class AddressViewModel
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        
    }
}
