using YourRest.Domain.Entities;

namespace YourRest.Application.Dto
{
    public class AddressDto
    {
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Type { get; set; }
        public int CityId { get; set; }
    }
}