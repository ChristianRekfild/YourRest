using YourRest.Domain.ValueObjects.Addresses;

namespace YourRest.Domain.Entities
{
    public class Address : IntBaseEntity
    {
        public City City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public AddressTypeVO Type { get; set; }
        public int CityId { get; set; }
    }
}
