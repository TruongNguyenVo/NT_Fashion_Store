using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using doan1_v1.Models;
using Microsoft.AspNetCore.Authorization;

namespace doan1_v1.Controllers
{
	[Authorize(Policy = "ManagerOnly")]
	public class OrdersController : Controller
    {
        private readonly NTFashionDbContext _context;

        public OrdersController(NTFashionDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Orders.ToListAsync());
        //}

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                                    .Include(o => o.Customer)
                                    .Include(o=> o.OrderProductDetails)
                                    .ToListAsync();
            ViewBag.Orders = orders;
            return View();
        }
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderProductDetails) // lay danh sach chi tiet order trong order
                        .ThenInclude(op => op.Product) // lay thong tin cua product trong danh sach chi tiet
                            .ThenInclude(opc => opc.Category) // lay ten category trong product
                    .FirstOrDefaultAsync();
            ViewBag.Order = order;
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateOrder,DateReceive,DeliveryCost,OtherCost,Status,Note,IsDel,AdminId,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.detailOrder = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderProductDetails) // lay danh sach chi tiet order trong order
                        .ThenInclude(op => op.Product) // lay thong tin cua product trong danh sach chi tiet
                            .ThenInclude(opc => opc.Category) // lay ten category trong product
                    .FirstOrDefaultAsync();
                // Lấy danh sách Category
                var categories = await _context.Categories.ToListAsync();
                ViewBag.Categories = categories;
            }
            return View(order);
        }


        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateOrder,DateReceive,DeliveryCost,OtherCost,Status,Note,IsDel,AdminId,CustomerId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                //_context.Orders.Remove(order);
                order.IsDel = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        //xác nhận đơn hàng giao thành công
        public async Task<IActionResult> confirmOrder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order != null) {
                order.Status = "Đã thanh toán"; // thay đổi trạng thái của đơn hàng
                order.DateReceive = DateOnly.FromDateTime(DateTime.Now);
                _context.Update(order);
                await _context.SaveChangesAsync();


            }
            else
            {
                return NotFound();
            }
            // về trang hiện tại (không di chuyển qua trang khác)
            return Redirect(Request.Headers["Referer"].ToString());
        }

        //get detail product
        //[HttpGet("{name}")]
        [Authorize(Policy = "ManagerOnly")]
        public async Task<IActionResult> GetProduct(string name)
        {
            var product = await _context.Products
                .Where(p => p.Name.Contains(name))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Color = p.Color,
                    Dimension = p.Dimension,
                    Material = p.Material,
                    Productor = p.Productor

                })
                .FirstOrDefaultAsync();
            //Console.WriteLine($"----------------{product.CategoryName}");
            //Console.WriteLine($"--------------{product?.Category.Name}-------");

            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            return Ok(product); // Return product as JSON
        }
       

    }
}
