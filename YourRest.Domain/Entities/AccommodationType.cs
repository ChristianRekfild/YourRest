namespace YourRest.Domain.Entities
{
    public class AccommodationType : IntBaseEntity
    {
        public string Name { get; set; }

        public List<Accommodation> Accommodations { get; set; }
    }
}
