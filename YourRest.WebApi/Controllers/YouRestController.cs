using Microsoft.AspNetCore.Mvc;

namespace YourRest.WebApi.Controllers
{
    public class YouRestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
