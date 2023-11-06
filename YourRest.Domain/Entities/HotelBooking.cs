using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourRest.Domain.Entities
{
    public class HotelBooking : IntBaseEntity 
    {
        public Accommodation Accommodation { get; set; }
        public int AccommodationId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int RoomId { get; set; }
        public decimal TotalAmount { get; set; }
        public int AdultNr { get; set; }
        public int ChildrenNr { get; set;}
    }
}
