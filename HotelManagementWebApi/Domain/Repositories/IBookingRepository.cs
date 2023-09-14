using HotelManagementWebApi.Domain.Entities.Booking;

namespace HotelManagementWebApi.Domain.Repositories;
public interface IBookingRepository
{
    Task<Booking> FindAsync(int id);

}