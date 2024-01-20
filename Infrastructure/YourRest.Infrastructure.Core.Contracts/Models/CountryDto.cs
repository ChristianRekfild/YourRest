namespace YourRest.Infrastructure.Core.Contracts.Models
{
    //TODO: разнести domain model и entity
    public class CountryDto : IntBaseEntityDto
    {       
        public string Name { get; set; }

        public List<RegionDto> Regions { get; set; } = new List<RegionDto>();
    }
}
