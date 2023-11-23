using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using YourRest.ClientWebApp.Models;

namespace YourRest.ClientWebApp.Views.Shared.Components
{
    public partial class SearchComponent
    {
        private IJSObjectReference module;
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Parameter]
        public DateTime StartDate { get; set; } = DateTime.Now;
        [Parameter]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(3);
        [Parameter]
        public int AdultNumber { get; set; } = 2;
        [Parameter]
        public int ChildrenNumber { get; set; } = 2;

        [Parameter]
        public int RoomCount { get; set; } = 1;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
           module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/components/search_component.js");
           await module.InvokeAsync<SelectedDateViewModel>("ShowDataPicker", StartDate.ToString("dd.MM.yyyyy"), EndDate.ToString("dd.MM.yyyyy"));
        }
        private async Task SetData()
        {
            var selectedDate = await module.InvokeAsync<SelectedDateViewModel>("GetData");
            StartDate = selectedDate.StartDate; 
            EndDate = selectedDate.EndDate;
        }
        private async Task GetData()
        {
            var selectedDate = await module.InvokeAsync<SelectedDateViewModel>("GetData");
            StartDate = selectedDate.StartDate;
            EndDate = selectedDate.EndDate;
            Navigation.NavigateTo("searchresult", true);
        }
        private async Task Show()
        {
            await module.InvokeVoidAsync("openForm");
        }
        private async Task Hide()
        {
            await module.InvokeVoidAsync("closeForm");
        }
    }
}
