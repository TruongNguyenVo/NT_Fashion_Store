using doan1_v1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace doan1_v1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
		public IActionResult Shop()
		{
			return View();
		}
		public IActionResult Profile()
		{
			return View();
		}
        public IActionResult Order()
        {
            return View();
        }
		public IActionResult Detail(int? id)
		{
			return View();
		}
		public IActionResult Contact()
		{
			return View();
		}
		public IActionResult Checkout()
		{
			return View();
		}
        public IActionResult Cart()
        {
            return View();
        }
		public IActionResult SignIn()
		{
			return View();
		}
        public IActionResult SignUp()
        {
            return View();
        }

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
