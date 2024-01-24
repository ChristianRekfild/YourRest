namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class AccommodationDto : IntBaseEntityDto
    {
        public string Name { get; set; }
        
        public string? Description { get; set; }
        public AddressDto? Address { get; set; }
        public int? AddressId { get; set; }
        
        public AccommodationTypeDto AccommodationType { get; set; }
        public int AccommodationTypeId { get; set; }
        public ICollection<RoomDto> Rooms { get; set; }
        public ICollection<UserAccommodationDto> UserAccommodations { get; set; } = new List<UserAccommodationDto>();
        public AccommodationStarRatingDto? StarRating { get; set; }
        
        public ICollection<AccommodationFacilityLinkDto> AccommodationFacilities { get; set; } = new List<AccommodationFacilityLinkDto>();
        public AccommodationDto()
        {
            Rooms = new List<RoomDto>();
        }
    }
}
