namespace YourRestDomain.Entities
{
    //TODO: разнести domain model и entity
    public class CountryEntity : BaseEntity<int>
    {
        public string Name { get; set; }
    }
}
