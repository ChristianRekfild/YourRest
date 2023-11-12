using Microsoft.AspNetCore.Mvc;
using YourRest.ClientWebApp.Models;
using YourRest.Domain.Entities;

namespace YourRest.ClientWebApp.Controllers
{
    public class HotelBookingController : Controller
    {

        public IActionResult index(HotelBookingClientModel booking)
        {

            return View();
        }
        public IActionResult Booking()
        {
            HotelBookingClientModel newBooking = new HotelBookingClientModel();

            Console.WriteLine($"Пришла бронь для отеля {newBooking.AccommodationId} комната {newBooking.RoomId}" +
            $"забронирована с {newBooking.DateFrom} по {newBooking.DateFrom}");

            return View();
        }
    }
}
