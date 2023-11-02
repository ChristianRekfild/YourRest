namespace YourRest.Domain.Models;

public class AccommodationFilterCriteria
{
    public List<int> CountryIds { get; set; }
    public List<int> CityIds { get; set; }
    public List<int> AccommodationTypesIds { get; set; }
}

