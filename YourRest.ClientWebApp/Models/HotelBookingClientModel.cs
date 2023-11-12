using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace YourRest.ClientWebApp.Models
{
    public class HotelBookingClientModel
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
