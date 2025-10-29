using FitnessLog.Presentation.Models;
using Microsoft.AspNetCore.Authorization; // << REQUIRED using statement
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FitnessLog.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize] // << REQUIRE LOGIN for the home page
        public IActionResult Index()
        {
            // Now, only logged-in users will reach this point.
            // Others are redirected to the Login page automatically.
            return View();
        }

        // The Privacy page can remain accessible without login
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}