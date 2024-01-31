namespace YourRest.ClientWebApp.Models
{
    public class BookingModel
    {
        public Guid SystemId { get; set; }
        public int ExternalId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int AdultNumber { get; set; }
        public int ChildrenNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? PassportNumber { get; set; }
        public List<int> Rooms { get; set; }
    }
}
