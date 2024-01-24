namespace YourRest.Producer.Infrastructure.Entities
{
    public class Customer : IntBaseEntity
    {
        public Guid SystemId { get; set; } = Guid.Empty;
        public int? ExternalId { get; set; } = null; 
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? PassportNumber { get; set; }
    }
}
