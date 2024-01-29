using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace YouRest.HotelierWebApp.Data.Models
{
    public class HotelModel
    {
        public int Id { get; set; }
        public int? AccommodationTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; }
        public HotelTypeModel? AccommodationType { get; set; }
        public List<RoomModel>? Rooms { get; set; }
        public AddressModel? Address { get; set; }
        public CityModel? City { get; set; }
        public RegionModel? Region { get; set; }
        public CountryModel? Country { get; set;}
    }
}
