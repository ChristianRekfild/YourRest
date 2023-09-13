namespace SharedKernel.Domain.Entities
{
	//TODO: разнести domain model и entity
	public class Region : IntBaseEntity
	{
		public string Name { get; set; }
		public int CountryId { get; set; }

		public virtual Country Country { get; set; }
	}
}
