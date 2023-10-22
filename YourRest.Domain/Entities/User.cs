namespace YourRest.Domain.Entities
{
    public class User : IntBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string KeyCloakId { get; set; }
        
        public ICollection<UserAccommodation> UserAccommodations { get; set; } = new List<UserAccommodation>();
    }
}
