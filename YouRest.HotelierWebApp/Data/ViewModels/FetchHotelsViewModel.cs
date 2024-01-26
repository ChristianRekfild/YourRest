namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class FetchHotelsViewModel
    {
        public List<int>? CountryIds { get; set; } = null;
        public List<int>? CityIds { get; set; } = null;
        public List<int>? AccommodationTypesIds { get; set; } = null;
        public List<int>? AccommodationFacilityIds { get; set; } = null;
        public List<int>? RoomFacilityIds { get; set; } = null;
    }
}
