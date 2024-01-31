using YourRest.Domain.Entities;

namespace YourRest.Domain.Service.AccommodationService
{
    public class AccommodationBuilder
    {
        private readonly string _name;
        private readonly AccommodationType _accommodationType;
        private string? _description;
        private Address? _address;
        private int? _addressId;
        private ICollection<Room> _rooms = new List<Room>();
        private ICollection<UserAccommodation> _userAccommodations = new List<UserAccommodation>();
        private AccommodationStarRating? _starRating;
        private ICollection<AccommodationFacilityLink> _accommodationFacilities = new List<AccommodationFacilityLink>();

        public AccommodationBuilder(string name, AccommodationType accommodationType)
        {
            _name = name;
            _accommodationType = accommodationType;
        }

        public AccommodationBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public AccommodationBuilder WithAddress(Address address, int addressId)
        {
            _address = address;
            _addressId = addressId;
            return this;
        }

        public AccommodationBuilder WithRooms(ICollection<Room> rooms)
        {
            _rooms = rooms;
            return this;
        }

        public AccommodationBuilder WithUserAccommodations(ICollection<UserAccommodation> userAccommodations)
        {
            _userAccommodations = userAccommodations;
            return this;
        }

        public AccommodationBuilder WithStarRating(AccommodationStarRating starRating)
        {
            _starRating = starRating;
            return this;
        }

        public AccommodationBuilder WithAccommodationFacilities(ICollection<AccommodationFacilityLink> accommodationFacilities)
        {
            _accommodationFacilities = accommodationFacilities;
            return this;
        }

        public Accommodation Build()
        {
            return new Accommodation
            {
                Name = _name,
                AccommodationType = _accommodationType,
                Description = _description,
                Address = _address,
                AddressId = _addressId,
                Rooms = _rooms,
                UserAccommodations = _userAccommodations,
                StarRating = _starRating,
                AccommodationFacilities = _accommodationFacilities
            };
        }
    }

}