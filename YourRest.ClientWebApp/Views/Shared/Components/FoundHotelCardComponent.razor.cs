using MassTransit;
using Microsoft.AspNetCore.Components;
using YouRest.HotelierWebApp.Data.Models;
using YourRest.Domain.Entities;

namespace YourRest.ClientWebApp.Views.Shared.Components
{
    public partial class FoundHotelCardComponent
    {
        [Inject] IPublishEndpoint PublishEndpoint { get; set; }
        [Parameter] public int StarCount { get; set; }
        [Parameter] public double Rating { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Description { get; set; }
        [Parameter] public double Price { get; set; }
        [Parameter] public string RoomUrl { get; set; }
        public async Task OnClick()
        {
            await PublishEndpoint.Publish<BookingModel>(new()
            {
                AdultNumber = 1,
                ChildrenNumber = 2,
                TotalAmount = 15000,
                Email = "grits@gmail.com",
                FirstName = "Ярослав",
                LastName = "Грицишин",
                MiddleName = "Владимирович",
                DateOfBirth = new DateTime(1989, 12, 27),
                ExternalId = 0,
                StartDate = new DateOnly(2024, 01, 31),
                EndDate = new DateOnly(2024, 02, 10),
                PhoneNumber = "+7(495)499 99 99",
                Rooms = new(),
                SystemId = Guid.NewGuid(),
                PassportNumber = 123456789
            });
        }
    }
}
