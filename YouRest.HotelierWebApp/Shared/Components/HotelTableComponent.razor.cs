using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using YouRest.HotelierWebApp.Data.Models;

namespace YouRest.HotelierWebApp.Shared.Components
{
    public partial class HotelTableComponent : ComponentBase
    {
        public string RowHoverStyle { get; set; }
        public string Edit { get; set; }
        public string Remove { get; set; }
        [Parameter] public ObservableCollection<HotelModel> Hotels { get; set; } 
        [Parameter] public EventCallback<ObservableCollection<HotelModel>> HotelsChanged { get; set; }
        [Parameter] public EventCallback<HotelModel> OnRemove { get; set; }
        [Parameter] public EventCallback<HotelModel> OnEdit { get; set; }
        [Parameter] public EventCallback<HotelModel> OnSelected { get; set; }
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
    }
}
