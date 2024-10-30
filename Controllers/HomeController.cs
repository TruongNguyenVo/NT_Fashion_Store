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
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Shop")]
        public IActionResult Shop()
		{
			return View();
		}
        [Route("Profile")]
        public IActionResult Profile()
		{
			return View();
		}
        [Route("Order")]
        public IActionResult Order()
        {
            return View();
        }
        [Route("product/detail/{id:int}")]
        public IActionResult Detail(int? id)
		{
            Console.WriteLine($"product id is: {id}");
			return View();
		}

        [Route("Contact")]
        public IActionResult Contact()
		{
			return View();
		}
        [Route("Checkout")]
        public IActionResult Checkout()
		{
			return View();
		}
        [Route("Cart")]
        public IActionResult Cart()
        {
            return View();
        }
        [Route("SignIn")]
        public IActionResult SignIn()
		{
			return View();
		}
        [Route("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }
        [Route("Privacy")]
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
