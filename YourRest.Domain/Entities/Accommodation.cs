using YourRest.Domain.Events;
using YourRest.Domain.ValueObjects;

namespace YourRest.Domain.Entities
{
    public class Accommodation : AbstractAggregateRoot
    {
        public string Name { get; set; }
        
        public string? Description { get; set; }
        public Address? Address { get; set; }
        public int? AddressId { get; set; }
        
        public int State { get; set; }
        public AccommodationType AccommodationType { get; set; }
        public int AccommodationTypeId { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<UserAccommodation> UserAccommodations { get; set; } = new List<UserAccommodation>();
        public AccommodationStarRating? StarRating { get; set; }
        
        public ICollection<AccommodationFacilityLink> AccommodationFacilities { get; set; } = new List<AccommodationFacilityLink>();
        public Accommodation()
        {
            AccommodationState state = AccommodationState.New;
            int stateValue = (int)state;
            
            State = stateValue;
            Rooms = new List<Room>();
            Record(new AccommodationCreatedEvent(1));
        }
    }
}
