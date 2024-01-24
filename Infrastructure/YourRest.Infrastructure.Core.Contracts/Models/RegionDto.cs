namespace YourRest.Infrastructure.Core.Contracts.Models
{
	public class RegionDto : IntBaseEntityDto
	{
		public string Name { get; set; }
		public int CountryId { get; set; }

		public virtual CountryDto Country { get; set; }

		public List<CityDto> Cities { get; set; } = new List<CityDto>();
	}
}
