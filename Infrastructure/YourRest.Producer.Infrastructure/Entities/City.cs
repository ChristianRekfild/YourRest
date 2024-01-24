namespace YourRest.Producer.Infrastructure.Entities
{
    public class City : IntBaseEntity
    {
        public string Name { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public ICollection<Address> Addresses { get; private set; }
        public City()
        {
            Addresses = new List<Address>();
        }
    }
}

