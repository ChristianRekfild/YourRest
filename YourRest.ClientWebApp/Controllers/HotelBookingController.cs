using MassTransit;
using Microsoft.AspNetCore.Mvc;
using YouRest.Booking.Contracts;
using YourRest.ClientWebApp.Models;

namespace YourRest.ClientWebApp.Controllers
{
    public class HotelBookingController : Controller
    {
        private readonly IPublishEndpoint publishEndpoint;

        public HotelBookingController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public IActionResult index(HotelBookingClientModel booking)
        {

            return View();
        }

        public async Task<IActionResult> Booking(HotelBookingClientModel newBooking)
        {
            //HotelBookingClientModel newBooking = new HotelBookingClientModel();
            if (newBooking.AccommodationId > 0)
            {
               await publishEndpoint.Publish<BookingSubmitted>(new
                {
                    CorrelationId = Guid.NewGuid(),
                    newBooking.AccommodationId,
                    StartDate = newBooking.DateFrom,
                    EndDate = newBooking.DateTo,
                    newBooking.TotalAmount,
                    newBooking.AdultNr,
                    newBooking.ChildrenNr
                });
                Console.WriteLine($"Пришла бронь для отеля {newBooking.AccommodationId} комната {newBooking.RoomId}" +
            $"забронирована с {newBooking.DateFrom} по {newBooking.DateFrom}");
            }


            return View();
        }
    }
}
