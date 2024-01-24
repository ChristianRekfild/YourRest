namespace YouRest.HotelierWebApp.Data.Models
{
    public class FetchHotelsModel
    {
        public List<int>? CountryIds { get; set; } = null;
        public List<int>? CityIds { get; set; } = null;
        public List<int>? AccommodationTypesIds { get; set; } = null;
        public List<int>? AccommodationFacilityIds { get; set; } = null;
        public List<int>? RoomFacilityIds { get; set; } = null;
    }
}
