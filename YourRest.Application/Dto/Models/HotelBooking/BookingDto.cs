using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourRest.Application.Dto.Models.HotelBooking
{
    public class BookingDto
    {
        public int AccommodationId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int RoomId { get; set; }
        public decimal TotalAmount { get; set; }
        public int AdultNr { get; set; }
        public int ChildrenNr { get; set; }
    }
}
