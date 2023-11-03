namespace YourRest.Application.Dto.ViewModels
{
    public class FetchAccommodationsViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int Adults { get; set; }
        public int? Children { get; set; }
        public List<int>? CountryIds { get; set; } = null;
        public List<int>? CityIds { get; set; } = null;
        public List<int>? AccommodationTypesIds { get; set; } = null;
        public List<int>? AccommodationFacilityIds { get; set; } = null;
        public List<int>? RoomFacilityIds { get; set; } = null;
    }
}
