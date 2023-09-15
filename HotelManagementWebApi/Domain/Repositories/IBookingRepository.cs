using HotelManagementWebApi.Domain.Entities.Booking;

namespace HotelManagementWebApi.Domain.Repositories;
public interface IBookingRepository : IPgRepository<Booking, int>
{

}