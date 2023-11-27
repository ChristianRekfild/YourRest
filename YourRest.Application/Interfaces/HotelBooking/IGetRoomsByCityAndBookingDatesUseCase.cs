using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourRest.Application.Dto;
using YourRest.Application.Dto.Models.HotelBooking;
using YourRest.Application.Dto.Models.Room;
using YourRest.Domain.ValueObjects.Bookings;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YourRest.Application.Interfaces.HotelBooking
{
    public interface IGetRoomsByCityAndBookingDatesUseCase
    {
        Task<List<RoomDto>> ExecuteAsync(DateOnly startDay, DateOnly endDay, int cityId , CancellationToken token = default);
    }
}
