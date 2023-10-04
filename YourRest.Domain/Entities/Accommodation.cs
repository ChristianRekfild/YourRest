namespace YourRest.Domain.Entities
{
    public class Accommodation : IntBaseEntity
    {
        public string Name { get; set; }
        public Address? Address { get; set; }
        public int? AddressId { get; set; }

    }
}
