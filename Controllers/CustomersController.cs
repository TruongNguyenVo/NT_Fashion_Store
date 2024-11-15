using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using doan1_v1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace doan1_v1.Controllers
{
    [Authorize(Policy = "ManagerOnly")]
    public class CustomersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly NTFashionDbContext _context;

        public CustomersController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, NTFashionDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            ////lay danh sach id cua role customer
            //var idRoleCustomer = _context.Roles.Where(r => r.Name == "Customer").FirstOrDefault();
            ////Console.WriteLine($"--------------------{idCustomer.Id}------------------");

            //if(idRoleCustomer != null)
            //{
            //    //lay danh sach id cua user co role la id customer
            //    var idCustomers = await _context.UserRoles.Where(ic => ic.RoleId == idRoleCustomer.Id).ToListAsync();
            //    var customerNames = await _context.Users
            //    .Where(u => idCustomers.Contains(u.Id))
            //    .Select(u => u.Name) // Thay "Name" bằng tên thuộc tính bạn muốn lấy
            //    .ToListAsync();
            //            }

            //lay danh sach cac customer
            var usersInCustomerRole = await _userManager.GetUsersInRoleAsync("Customer");
            ViewBag.users = usersInCustomerRole;
            return View();

                                            
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await _context.Users.Where(c => c.Id == id)
                .FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.user = customer;
            return View();
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Phone,DateOfBrith,Gender,FullName,Address,Email,Password,IsDel")] Customer customer)
        {
            //Console.WriteLine();
            if (ModelState.IsValid)
            {
                //tới đây rồi nè 
                var newCustomer = new Customer { 
                    FullName = customer.FullName,
                    DateOfBrith = customer.DateOfBrith,
                    Gender = customer.Gender,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    //UserName = UserName,
                    Address = customer.Address
                };
                _context.Add(customer);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Users.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.user = customer; //thong tin cua khahc hang
            ViewBag.gender = new List<string> { "Nam", "Nữ", "Không xác định" };
            return View();
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, string FullName, string Gender, string PhoneNumber, string Email, DateOnly DateOfBrith, string Address)
        {
            //if (id != customer.Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByIdAsync(Id);
                try
                {

                    if (existingUser != null) {
                        existingUser.FullName = FullName;
                        existingUser.Gender = Gender;
                        existingUser.PhoneNumber = PhoneNumber;
                        existingUser.Email = Email;
                        existingUser.DateOfBrith = DateOfBrith;
                        existingUser.Address = Address;
                        _context.Update(existingUser);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(existingUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        //// GET: Customers/Delete/5
        //public async Task<IActionResult> Delete(string? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customers
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customer);
        //}

        //// POST: Customers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var customer = await _context.Customers.FindAsync(id);
        //    if (customer != null)
        //    {
        //        _context.Customers.Remove(customer);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        //// POST: Customers/Delete/5
        [HttpGet]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var customer = await _userManager.FindByIdAsync(id);
            if (customer != null)
            {
                customer.IsDel = true;
                //_context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(string id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
        private string generateUsername(string FullName)
        {
            if (string.IsNullOrEmpty(FullName))
            {

            }
            return "";
        }
    }
}
