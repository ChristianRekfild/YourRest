namespace YourRest.Domain.Entities
{
	public class Region : IntBaseEntity
	{
		public string Name { get; set; }
		public int CountryId { get; set; }

		public virtual Country Country { get; set; }

		public List<City> Cities { get; set; } = new List<City>();
	}
}
