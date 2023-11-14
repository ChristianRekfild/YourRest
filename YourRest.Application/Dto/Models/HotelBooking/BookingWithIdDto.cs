using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourRest.Application.Dto.Models.HotelBooking
{
    public class BookingWithIdDto : BookingDto
    {
        public int Id { get; set; }
    }
}
