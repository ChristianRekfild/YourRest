using HotelManagementWebApi.Tests.Fixtures;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Repositories;
using YourRest.Infrastructure.Repositories;

namespace HotelManagementWebApi.Tests.Repositories
{
    [Collection("Database")]
    public class RepositoryTests
    {
        private readonly IBookingRepository bookingRepository;
        private readonly ICustomerRepository customerRepository;

        public RepositoryTests(DatabaseFixture databaseFixture) {
            bookingRepository = new BookingRepository(databaseFixture.DbContext);
            customerRepository = new CustomerRepository(databaseFixture.DbContext);
        }

        [Fact]
        public async void EmptyRepositoryFindTest()
        {
            var bookings = await bookingRepository.FindAsync(x => x.Id == 0);

            Assert.Empty(bookings);
        }

        [Fact]
        public async void RepositoryFindByNameTest()
        {
            var customer = await customerRepository.AddAsync(
                new Customer {
                    FirstName = "Иван",
                    LastName = "Иванов",
                    MiddleName = "Иванович",
                    IsActive = true,
                    Login = "ivanov@ivan.com",
                    Password = "qwerty"
                }); 

            var booking1 = new Booking {
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(20),
                Status = BookingStatus.Pending,
                Comment = "Первый тестовый комментарий",
                CustomerId = customer.Id                
            };
            var booking2 = new Booking
            {
                StartDate = DateTime.Now.AddDays(4),
                EndDate = DateTime.Now.AddDays(6),
                Status = BookingStatus.Pending,
                Comment = "Второй тестовый комментарий",
                CustomerId = customer.Id
            };
            var booking3 = new Booking
            {
                StartDate = DateTime.Now.AddDays(7),
                EndDate = DateTime.Now.AddDays(23),
                Status = BookingStatus.Pending,
                Comment = "Третий тестовый комментарий",
                CustomerId = customer.Id
            };

            await bookingRepository.AddRangeAsync(new[] { booking1, booking2, booking3 });            

            var bookings = await bookingRepository.FindAsync(x => x.Comment.IndexOf("тестовый комментарий") >= 0);

            Assert.Equal(3, bookings.Count());

            bookings = await bookingRepository.FindAsync(x => x.Comment.IndexOf("Третий") >= 0);

            Assert.Single(bookings);

            bookings = await bookingRepository.FindAsync(x => x.Comment.IndexOf("мимо") >= 0);

            Assert.Empty(bookings);
        }
    }
}
