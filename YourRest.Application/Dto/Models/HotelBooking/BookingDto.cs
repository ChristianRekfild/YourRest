using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto.Models.Room;

namespace YourRest.Application.Dto.Models.HotelBooking
{
    public class BookingDto
    {
        public Guid SystemId { get; set; } = Guid.Empty;
        public int ExternalId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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
        public List<RoomWithIdDto> Rooms { get; set; } = new();
    }
}
