using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class HotelViewModel
    {
        public int Id { get; set; }
        public int AccommodationTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stars { get; set; }
    }
}
