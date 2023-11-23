using Microsoft.AspNetCore.Components;

namespace YourRest.ClientWebApp.Views.Shared.Components
{
    public partial class FoundHotelCardComponent
    {
        [Parameter] public int StarCount { get; set; } = 5;
        [Parameter] public double Rating { get; set; } = 9.2;
        [Parameter] public string Title { get; set; } = "Русотель";
        [Parameter] public string Description { get; set; } = "Этот уютный отель расположен в 10 минутах ходьбы от железнодорожной станции Битца, в 12 км от парка \"Битцевский лес\" и в 14 км от музея-заповедника \"Царицыно\" с дворцом XVIII века.";
    }
}
