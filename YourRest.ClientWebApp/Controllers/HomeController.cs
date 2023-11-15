using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using YourRest.ClientWebApp.Models;

namespace YourRest.ClientWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //test
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetDataFormJS(SelectedDateViewModel selectedDate)
        {
            return View("Index");
        }

        public IActionResult Login(LoginViewModel data)
        {
            return View("Index");
        }
        public IActionResult Register(RegisterViewModel data)
        {
            return View("Privacy");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}