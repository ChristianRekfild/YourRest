namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class AccommodationFilterCriteriaDto
    {
        public List<int> CountryIds { get; set; }
        public List<int> CityIds { get; set; }
        public List<int> AccommodationTypesIds { get; set; }
    }
}
