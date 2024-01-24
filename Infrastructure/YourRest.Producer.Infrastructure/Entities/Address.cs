namespace YourRest.Producer.Infrastructure.Entities
{
    public class Address : IntBaseEntity
    {
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
    }
}
