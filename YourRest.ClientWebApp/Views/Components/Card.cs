using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace YourRest.ClientWebApp.Views.Components
{
    [ViewComponent]
    public class Card
    {
        public IViewComponentResult Invoke()
        {
            return new HtmlContentViewComponentResult(
                new HtmlString(""));
        }
    }
}
