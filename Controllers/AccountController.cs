using doan1_v1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;

namespace doan1_v1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
		private readonly NTFashionDbContext _context;
		public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, NTFashionDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]

        [Route("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            //xem thong tin cua tai khoan
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
            if (result.Succeeded) { 
                var user = await _userManager.FindByNameAsync(username);
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Customer")) //neu la customer
                {
                    return RedirectToAction("Index", "Home"); //thi chay den trang chu
                }
            }
            return View();
        }
        [HttpGet]
        [Route("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string FullName, DateOnly DateOfBrith, string Gender, string Email, string Phone, string UserName, string Password, string ConfirmPassword, string Address)
        {
            //Console.WriteLine("Vao day roi");
            //tạo customer

            var customer = new Customer()
            {
                FullName = FullName,
                DateOfBrith = DateOfBrith,
                Gender = Gender,
                Email = Email,
                Phone = Phone,
                UserName = UserName,
                Address = Address
            };
            //tạo người dùng
            var result = await _userManager.CreateAsync(customer, Password);
            // Kiểm tra xem vai trò "Customer" đã tồn tại hay chưa
            if (!await _roleManager.RoleExistsAsync("Customer"))
            {
                // Nếu chưa tồn tại, tạo vai trò "Customer"
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(customer, "Customer");
                if (roleResult.Succeeded) {
                    ////khi tạo tài khoản thành công thì tạo một cart luôn
                    Cart cart = new Cart
                    {
                        UserId = customer.Id
                    };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();

                }
                // Đăng nhập ngay sau khi tạo tài khoản
                await _signInManager.SignInAsync(customer, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
                return RedirectToAction("Index");
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult AccessDenied()
        {
            return View(); // Tạo một view AccessDenied.cshtml để hiển thị thông báo
        }
        // Đăng xuất
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();  // Xóa cookie đăng nhập
            return RedirectToAction("Index", "Home");  // Chuyển hướng về trang chủ
        }
    }
}
