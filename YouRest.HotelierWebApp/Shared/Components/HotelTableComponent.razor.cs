using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Shared.Components
{
    public partial class HotelTableComponent: ComponentBase
    {
        public string RowHoverStyle { get; set; }
        public string Edit { get; set; }
        public string Remove { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }
        [Parameter] public List<HotelViewModel> Hotels { get; set; }
        [Parameter] public EventCallback<List<HotelViewModel>> HotelsChanged { get; set; }
        protected override void OnAfterRender(bool firstRender)
        {

            base.OnAfterRender(firstRender);
        }
        public void MouseOverOfEditBtn()
        {
            Edit = "edit";
            RowHoverStyle = "border-bottom: 1px solid #fd7e14 !important;\r\n    background-color: RGBA(253,126,20,0.1) !important;\r\n    color: #333 !important;";

        }
        public void MouseOutOfEditBtn()
        {
            Edit = string.Empty;
            RowHoverStyle = string.Empty;
        }
        public void MouseOverOfRemoveBtn()
        {
            Remove = "remove";
            RowHoverStyle = $"border-bottom: 1px solid RGBA(220,53,69,1) !important;\r\n    background-color: RGBA(220,53,69,0.09) !important;\r\n    color: #333 !important;";
        }
        public void MouseOutOfRemoveBtn()
        {
            Remove = string.Empty;
            RowHoverStyle = string.Empty;
        }
        public async Task OnRemove(HotelViewModel hotel)
        {
            Hotels.Remove(hotel);
            await HotelsChanged.InvokeAsync(Hotels);
        }
        public void NavigateToEdit() => Navigation.NavigateTo("hotels/edit");
        protected void SelectedItem(HotelViewModel hotel)
        {
            Navigation.NavigateTo($"/hotels/{hotel.Id}");
        }
    }
}
