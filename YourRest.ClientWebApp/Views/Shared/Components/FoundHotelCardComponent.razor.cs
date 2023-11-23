using Microsoft.AspNetCore.Components;

namespace YourRest.ClientWebApp.Views.Shared.Components
{
    public partial class FoundHotelCardComponent
    {
        [Parameter] public int StarCount { get; set; } 
        [Parameter] public double Rating { get; set; }
        [Parameter] public string Title { get; set; } 
        [Parameter] public string Description { get; set; } 
        [Parameter] public double Price { get; set; }
        [Parameter] public string RoomUrl { get; set; }
    }
}
