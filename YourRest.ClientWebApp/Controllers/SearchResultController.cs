using Microsoft.AspNetCore.Mvc;

namespace YourRest.ClientWebApp.Controllers
{
    public class SearchResultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
