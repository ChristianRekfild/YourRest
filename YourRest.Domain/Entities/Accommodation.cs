namespace YourRest.Domain.Entities
{
    public class Accommodation : IntBaseEntity
    {
        public Accommodation()
        {
            Rooms = new List<Room>();
            UserAccommodations = new List<UserAccommodation>();
            AccommodationPhotos = new List<AccommodationPhoto>();
            AccommodationFacilities = new List<AccommodationFacilityLink>();
        }
        public string Name { get; set; }
        public string? Description { get; set; }
        public AccommodationStarRating? StarRating { get; set; }
       

        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public int AccommodationTypeId { get; set; }
        public AccommodationType AccommodationType { get; set; }
        
        public ICollection<Room> Rooms { get; set; }
        public ICollection<UserAccommodation> UserAccommodations { get; set; } 
        public ICollection<AccommodationPhoto> AccommodationPhotos { get; set; } 
        public ICollection<AccommodationFacilityLink> AccommodationFacilities { get; set; } 

    }
}
