using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NuGet.Packaging.Licenses;
using YourRest.ClientWebApp.Models;

namespace YourRest.ClientWebApp.Views.Shared.Components
{
    public partial class SearchComponent
    {
        private IJSObjectReference module;
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Parameter]
        public DateTime StartDate { get; set; }
        [Parameter]
        public DateTime EndDate { get; set; }
        [Parameter]
        public int AdultNumber { get; set; } = 2;
        [Parameter]
        public int ChildrenNumber { get; set; } = 2;

        [Parameter]
        public int RoomCount { get; set; } = 1;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
           module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Views/Shared/Components/SearchComponent.razor.js");
           await module.InvokeAsync<SelectedDateViewModel>("ShowDataPicker");
        }
        private async Task GetData()
        {
            var a = await module.InvokeAsync<SelectedDateViewModel>("GetData");
        }
    }
}
