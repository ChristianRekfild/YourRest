using Newtonsoft.Json;

namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class HotelViewModel
    {
        public int Id { get; set; }
        [JsonProperty("AccommodationTypeId")]
        public int HotelTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; }
    }
}
