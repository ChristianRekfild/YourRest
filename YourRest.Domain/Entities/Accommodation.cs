namespace YourRest.Domain.Entities
{
    public class Accommodation : IntBaseEntity
    {
        public string Name { get; set; }
        public List<Address> Addresses { get; set; }

        public Accommodation()
        {
            Addresses = new List<Address>();
        }
    }
}
