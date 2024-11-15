using doan1_v1.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "ManagerOrCustomer")]
        [Route("Profile")]
        public IActionResult Profile()
        {
            ViewBag.Genders = new List<string> { "Nam", "Nữ", "Không xác định"};
			return View();
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
                else if (roles.Contains("Manager")) // neu la manager 
                {
                    return RedirectToAction("SaleReport", "Invoices"); // thi den trang thong ke doanh thu
                }
            }
            return View("SignIn");
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
                PhoneNumber = Phone,
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
                if (roleResult.Succeeded)
                {
                    ////khi tạo tài khoản thành công thì tạo một cart luôn
                    Cart cart = new Cart
                    {
                        UserId = customer.Id
                    };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();

                    // Đăng nhập ngay sau khi tạo tài khoản
                    await _signInManager.SignInAsync(customer, isPersistent: false);
                    return RedirectToAction("Index", "Home");

                }
            }




				//var manager = new Manager()
				//{
				//	FullName = FullName,
				//	Email = Email,
				//	UserName = UserName,
				//	Address = Address,

				//};
				////tạo manager
				//var result = await _userManager.CreateAsync(manager, Password);
				//         // Kiểm tra xem vai trò "Customer" đã tồn tại hay chưa
				//         if (!await _roleManager.RoleExistsAsync("Manager"))
				//         {
				//             // Nếu chưa tồn tại, tạo vai trò "Customer"
				//             await _roleManager.CreateAsync(new IdentityRole("Manager"));
				//         }
				//         if (result.Succeeded)
				//         {
				//             var roleResult = await _userManager.AddToRoleAsync(manager, "Manager");
				//             if (roleResult.Succeeded)
				//             {
				//                 ////khi tạo tài khoản thành công thì tạo một cart luôn
				//                 Cart cart = new Cart
				//                 {
				//                     UserId = manager.Id
				//                 };
				//                 _context.Carts.Add(cart);
				//                 await _context.SaveChangesAsync();

				//    }
				//    // Đăng nhập ngay sau khi tạo tài khoản
				//    await _signInManager.SignInAsync(manager, isPersistent: false);
				//    return RedirectToAction("Index", "Home");
				//}

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
        [Authorize(Policy = "ManagerOrCustomer")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();  // Xóa cookie đăng nhập
            return RedirectToAction("Index", "Home");  // Chuyển hướng về trang chủ
        }

        //Đổi thông tin cá nhân
        [Authorize(Policy = "ManagerOrCustomer")]
        [HttpPost]
        public async Task<IActionResult> ChangeInforAsync(string userId, string FullName, string Email, string PhoneNumber, string Address, string Gender, DateOnly DateOfBirth)
        {
            Console.WriteLine();
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null) {
                return NotFound();
            }
            else
            {
               existingUser.FullName = FullName;
                existingUser.Email = Email;
                existingUser.PhoneNumber = PhoneNumber;
                existingUser.Address = Address;
                existingUser.Gender = Gender;
                existingUser.DateOfBrith = DateOfBirth;

                //luu thay doi
                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded) {
                    Console.WriteLine("Luu thanh cong");
                    return RedirectToAction("Profile");
                }
                else
                {
                    Console.WriteLine("Loi roi");
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }
            return View("Profile");
        }
        //Đổi mật khẩu
        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmNewPassword)
        {
            Console.WriteLine();
            return View("Profile");
        }
    }
}
