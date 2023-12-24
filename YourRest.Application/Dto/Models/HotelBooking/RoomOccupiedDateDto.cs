using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourRest.Application.Dto.Models.HotelBooking
{
    public class RoomOccupiedDateDto 
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set;
        }
    }
}
