namespace YourRest.Domain.Entities
{
    public class UserAccommodation : IntBaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
    }

}
