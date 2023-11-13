namespace YourRest.Domain.Entities
{
    public class Guest : IntBaseEntity
    {
        public Guid SystemId { get; set; } = Guid.Empty;
        public int ExternalId { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? PassportNumber { get; set; }
    }
}
