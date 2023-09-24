using Microsoft.AspNetCore.Mvc;

namespace HotelManagementWebApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
