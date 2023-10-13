namespace YourRest.Domain.Entities
{
    public class User : IntBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Accommodation Accommodation { get; set; }
        public string KeyCloakId { get; set; }
        public int AccommodationId { get; set; }
    }
}
