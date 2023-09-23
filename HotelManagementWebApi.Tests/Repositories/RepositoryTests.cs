using HotelManagementWebApi.Domain.Entities;
using HotelManagementWebApi.Domain.Repositories;
using HotelManagementWebApi.Infrastructure.Repositories;
using HotelManagementWebApi.Tests.Fixtures;

namespace HotelManagementWebApi.Tests.Repositories
{
    [Collection("Database")]
    public class RepositoryTests
    {
        private IBookingRepository repository;

        public RepositoryTests(DatabaseFixture databaseFixture) {
            repository = new BookingRepository(databaseFixture.DbContext);
        }

        [Fact]
        public async void EmptyRepositoryFindTest()
        {
            var bookings = await repository.FindAsync(x => x.Id == 0);

            Assert.Empty(bookings);
        }

        [Fact]
        public async void RepositoryFindByNameTest()
        {
            var booking1 = new Booking {
                StartDate = new Domain.ValueObjects.Bookings.BookingDate(DateTime.Now.AddDays(2)),
                EndDate = new Domain.ValueObjects.Bookings.BookingDate(DateTime.Now.AddDays(20)),
                Status = new Domain.ValueObjects.Bookings.BookingStatus(1),
                Comment = "Первый тестовый комментарий"
            };
            var booking2 = new Booking
            {
                StartDate = new Domain.ValueObjects.Bookings.BookingDate(DateTime.Now.AddDays(4)),
                EndDate = new Domain.ValueObjects.Bookings.BookingDate(DateTime.Now.AddDays(6)),
                Status = new Domain.ValueObjects.Bookings.BookingStatus(1),
                Comment = "Второй тестовый комментарий"
            };
            var booking3 = new Booking
            {
                StartDate = new Domain.ValueObjects.Bookings.BookingDate(DateTime.Now.AddDays(7)),
                EndDate = new Domain.ValueObjects.Bookings.BookingDate(DateTime.Now.AddDays(23)),
                Status = new Domain.ValueObjects.Bookings.BookingStatus(1),
                Comment = "Третий тестовый комментарий"
            };

            await repository.AddRangeAsync(new[] { booking1, booking2, booking3 });            

            var bookings = await repository.FindAsync(x => x.Comment.IndexOf("тестовый комментарий") >= 0);

            Assert.Equal(3, bookings.Count());

            bookings = await repository.FindAsync(x => x.Comment.IndexOf("Третий") >= 0);

            Assert.Single(bookings);

            bookings = await repository.FindAsync(x => x.Comment.IndexOf("мимо") >= 0);

            Assert.Empty(bookings);
        }
    }
}
