namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class CityDto : IntBaseEntityDto
    {
        public string Name { get; set; }
        public int RegionId { get; set; }
        public RegionDto Region { get; set; }
        public ICollection<AddressDto> Addresses { get; private set; }
        public CityDto()
        {
            Addresses = new List<AddressDto>();
        }
    }
}

