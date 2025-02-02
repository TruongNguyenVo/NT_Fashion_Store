using doan1_v1.Models;
using doan1_v1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using System.Text.RegularExpressions;

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
        public async Task<IActionResult> LogIn(SignInViewModel signInViewModel)
        {
            if (ModelState.IsValid)
            {
                //nếu tài khoản đã bị xóa thì thông báo

                if (await _userManager.Users.AnyAsync(u => u.IsDel && u.UserName == signInViewModel.Username))
                {
					ModelState.AddModelError("","Tài khoản người dùng đã bị vô hiệu hóa .");
					return View("SignIn",signInViewModel);
				}

				//xem thong tin cua tai khoan
				//var result = await _signInManager.
				//            PasswordSignInAsync(signInViewModel.Username, signInViewModel.Password, false, false);
				var result = await _signInManager.
							PasswordSignInAsync(signInViewModel.Username, signInViewModel.Password, false, false);
				if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(signInViewModel.Username);
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
                else
                {
                    //Console.WriteLine($"------------{signInViewModel.Username}------{signInViewModel.Password}");
                    //Console.WriteLine("Sai tai khoan hoac mat khau");
					ModelState.AddModelError("", "Tài khoản hoặc mật khẩu bị sai. Vui lòng kiểm tra lại");
					return View("SignIn", signInViewModel);
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
        public async Task<IActionResult> Register(SignUpViewModel signUpViewModel)
        {
            //Console.WriteLine("Vao day roi");
            //tạo customer
            if (ModelState.IsValid)
            {
                // Kiểm tra xem số điện thoại đã tồn tại trong hệ thống chưa
                if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == signUpViewModel.PhoneNumber))
                {
                    ModelState.AddModelError("", "Số điện thoại đã được sử dụng.");
                    return View(signUpViewModel);
                }
                // Kiểm tra xem email đã tồn tại trong hệ thống chưa
                if (await _userManager.Users.AnyAsync(u => u.Email == signUpViewModel.Email))
                {
                    ModelState.AddModelError("", "Email đã được sử dụng.");
                    return View("SignUp", signUpViewModel);
                }
                //kiểm tra username có tồn tại
                if (await _userManager.Users.AnyAsync(u => u.UserName == signUpViewModel.UserName))
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại.");
                    return View("SignUp", signUpViewModel);
                }
                //kiểm tra password có đúng chuẩn không
                if (!IsPasswordValid(signUpViewModel.Password))
                {
                    // Return an error message if the password is invalid
                    ModelState.AddModelError("Mật khẩu", "Mật khẩu của bạn phải theo chuẩn sau: " +
                        "ít nhất 10 ký tự, " +
                        "một chữ in hoa, " +
                        "một chữ thường, " +
                        "một con số, và " +
                        "một ký tự đặc biệt (ví dụ !@#$%^&*.)");
                    return View("SignUp", signUpViewModel);
                }
				//nếu trong bảng users không có record nào thì mặc định người tạo đầu tiên là admin
				bool isHaveUser = _context.Users.Any();
				string role = "Customer";
				if (!isHaveUser)
				{
					role = "Manager"; // neu khong co record nao thi nguoi dau tien la admin
				}

				var customer = new Customer()
                {

                    FullName = signUpViewModel.FullName,
                    DateOfBrith = signUpViewModel.DateOfBrith,
                    Gender = signUpViewModel.Gender,
                    Email = signUpViewModel.Email,
                    PhoneNumber = signUpViewModel.PhoneNumber,
                    UserName = signUpViewModel.UserName,
                    Address = signUpViewModel.Address
                };
                //tạo người dùng
                var result = await _userManager.CreateAsync(customer, signUpViewModel.Password);
                // Kiểm tra xem vai trò "Customer" đã tồn tại hay chưa
                if (!await _roleManager.RoleExistsAsync("Customer"))
                {
                    // Nếu chưa tồn tại, tạo vai trò "Customer"
                    await _roleManager.CreateAsync(new IdentityRole("Customer"));
                }
                // Kiểm tra xem vai trò "Customer" đã tồn tại hay chưa
                if (!await _roleManager.RoleExistsAsync("Manager"))
                {
                    // Nếu chưa tồn tại, tạo vai trò "Customer"
                    await _roleManager.CreateAsync(new IdentityRole("Manager"));
                }

                if (result.Succeeded)
                {
                    //them role
                    var roleResult = await _userManager.AddToRoleAsync(customer, role);
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
            }
				return View("SignUp");
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
            //Console.WriteLine();
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
		[Authorize(Policy = "ManagerOrCustomer")]
		//Đổi mật khẩu
		[HttpPost]
        public async Task<IActionResult> ChangePassword(string userId, string oldPassword, string newPassword, string confirmNewPassword)
        {
			//Console.WriteLine();
			var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound();
            }
            else
            {
                Console.WriteLine();
                var result = await _userManager.ChangePasswordAsync(existingUser, oldPassword, newPassword);
                if (result.Succeeded)
                {
                    //change password thanh cong
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home"); // chuyen den trang chu
                }
                
            }
			return View("Profile");
		}
        //[HttpGet]
        //public bool IsEmailExists(string email)
        //{
        //    //// Check if the email exists in the database
        //    //var emailExists = _context.Users.Any(u => u.Email == email);
        //    Console.WriteLine();
        //    //// Return true if the email is already taken (invalid), otherwise false (valid)
        //    //if (emailExists)
        //    //{
        //    //    return Json($"The email {email} is already in use.");
        //    //}
        //    //return Json(true); // True indicates that the email is valid
        //    return false;
        //}
        private bool IsPasswordValid(string password)
        {
            // Minimum length of 10 characters
            if (password.Length < 10)
                return false;

            // Regular expressions for the required password components
            var hasUpperCase = new Regex(@"[A-Z]"); // At least one uppercase letter
            var hasLowerCase = new Regex(@"[a-z]"); // At least one lowercase letter
            var hasNumber = new Regex(@"[0-9]"); // At least one number
            var hasSpecialChar = new Regex(@"[!@#$%^&*]"); // At least one special character

            // Check all conditions
            return hasUpperCase.IsMatch(password) &&
                   hasLowerCase.IsMatch(password) &&
                   hasNumber.IsMatch(password) &&
                   hasSpecialChar.IsMatch(password);
        }
        [HttpPost]
		[Authorize(Policy = "ManagerOrCustomer")]
		public async Task<IActionResult> deleteAccount(string userId)
        {
            //Console.WriteLine();
			//tim den user
			var existingUser = await _userManager.FindByIdAsync(userId);
            if(existingUser != null)
            {
                existingUser.IsDel = true;
                await _userManager.UpdateAsync(existingUser);
				await _signInManager.SignOutAsync();  // Xóa cookie đăng nhập
			}
			//Console.WriteLine();
			return RedirectToAction("Index", "Home");
        }
    }
}
