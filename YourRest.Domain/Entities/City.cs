namespace YourRest.Domain.Entities
{
    public class City : IntBaseEntity
    {
        public City() => Addresses = new List<Address>();
        
        public string Name { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }
        ICollection<Address> Addresses { get; set;}
    }
}

