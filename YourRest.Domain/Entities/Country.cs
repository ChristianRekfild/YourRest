namespace YourRest.Domain.Entities
{
    //TODO: разнести domain model и entity
    public class Country : IntBaseEntity
    {       
        public string Name { get; set; }

        public List<Region> Regions { get; set; } = new List<Region>();
    }
}
