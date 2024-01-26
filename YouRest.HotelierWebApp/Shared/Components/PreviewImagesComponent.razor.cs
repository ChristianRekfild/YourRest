using Microsoft.AspNetCore.Components;
using YouRest.HotelierWebApp.Data.ViewModels;

namespace YouRest.HotelierWebApp.Shared.Components
{
    public partial class PreviewImagesComponent : ComponentBase
    {
        [Parameter] public List<string> Images { get; set; } = new();
        [Parameter] public EventCallback<List<string>> ImagesChanged { get; set; }
        private string BorderStyle = string.Empty;
        public void MouseOverOfDeleteBtn() => BorderStyle = "border: 1.5px solid red;";
        public void MouseOutOfRemoveBtn() => BorderStyle = string.Empty;
        
        public async Task OnRemove(string item)
        {
            Images.Remove(item);
           await ImagesChanged.InvokeAsync(Images);
        }
    }
}
