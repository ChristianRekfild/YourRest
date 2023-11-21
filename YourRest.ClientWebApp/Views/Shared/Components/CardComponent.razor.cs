using Microsoft.AspNetCore.Components;

namespace YourRest.ClientWebApp.Views.Shared.Components
{
    public partial class CardComponent
    {
        [Parameter]
        public string CarouseControlId { get; set; }
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public string Description { get; set; }
        [Parameter]
        public double Price { get; set; }
        [Parameter]
        public List<string> ImageUrls { get; set; } = new();

    }
}
