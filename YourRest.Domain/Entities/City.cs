namespace YourRest.Domain.Entities
{
    public class City : IntBaseEntity
    {
        public string Name { get; set; }
        
        public string? Description { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        
        public bool IsFavorite { get; set; }
        public ICollection<Address> Addresses { get; private set; }
        public City()
        {
            Addresses = new List<Address>();
        }
        
        public ICollection<CityPhoto> CityPhotos { get; set; } = new List<CityPhoto>();

    }
}

