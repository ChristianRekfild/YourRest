namespace YourRestDomain.Entities
{
    //TODO: разнести domain model и entity
    public class RegionEntity : BaseEntity<int>
    {
        public string Name { get; set; }
        public int CountryId { get; set; }

        public virtual CountryEntity Country { get; set; }
    }
}
