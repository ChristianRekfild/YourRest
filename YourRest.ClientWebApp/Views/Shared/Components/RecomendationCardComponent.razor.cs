using Microsoft.AspNetCore.Components;

namespace YourRest.ClientWebApp.Views.Shared.Components
{
    public partial class RecomendationCardComponent
    {
        [Parameter]
        public string Url { get; set; }
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public string Description { get; set; }
    }
}
