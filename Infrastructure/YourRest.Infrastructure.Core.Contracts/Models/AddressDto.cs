namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class AddressDto : IntBaseEntityDto
    {
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public int CityId { get; set; }
        public CityDto City { get; set; }
    }
}
