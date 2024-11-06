using doan1_v1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Diagnostics;

namespace doan1_v1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NTFashionDbContext _context;

        public HomeController(ILogger<HomeController> logger, NTFashionDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [Route("")]
        public async Task<IActionResult> Index()
        {
            //viet de lay 8 san pham (4 quan, 4 ao)

            //ao
            var productAos = await _context.Products
                                            .Where(p => p.CategoryId == 4)
                                            .Include(p=> p.ProductImages)
                                            .Take(4)
                                            .ToListAsync();
            //quan
            var productQuans = await _context.Products
                                            .Where(p=> p.CategoryId ==5)
                                            .Include(p=> p.ProductImages)
                                            .Take(4)
                                            .ToListAsync();
            ViewBag.ProductAos = productAos;
            ViewBag.ProductQuans = productQuans;
            return View();
        }
        [Route("Shop/{id?}")]
        public async Task<IActionResult> Shop(int? id)
		{
			//danh muc san pham
			var categories = await _context.Categories
								.Where(c => c.ParentId != null)
								.ToListAsync();
			ViewBag.categories = categories;
			// lay tat ca san pham
			if (id == null)
            {
				var products = await _context.Products
					.Where(p => p.IsDel == false)
                    .Include(p=> p.ProductImages)
					.ToListAsync();
				ViewBag.products = products;
				return View();
            }
            //lay san pham theo category
            else
            {
                var products = await _context.Products
                                    .Where(p=>p.CategoryId == id && p.IsDel == false)
                                    .Include(p=> p.ProductImages)
                                    .ToListAsync();
                ViewBag.products = products;
                return View();
            }
			
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
        public async Task<IActionResult> Detail(int id)
		{
            // san pham chinh
            var product = await _context.Products.
                           Where(p => p.Id == id)
                           .Include (p => p.ProductImages)
                           .Include(p=> p.Category)
						   .FirstOrDefaultAsync();
            ViewBag.product = product;
            //san pham tuong tu
            var relativeProducts = await _context.Products
                                    .Where(p=> p.CategoryId == product.CategoryId)
                                    .Include(p=> p.ProductImages)
                                    .Include(p=>p.Category)
                                    .Take(3)
                                    .ToListAsync();
            ViewBag.relativeProducts = relativeProducts;

            //danh muc san pham
            var categories = await _context.Categories
                                .Where(c => c.ParentId != null)
                                .ToListAsync();
            ViewBag.categories = categories;
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
