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
	public class InvoicesController : Controller
    {
        private readonly NTFashionDbContext _context;

        public InvoicesController(NTFashionDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Orders.ToListAsync());
        //}
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                        .Where(o=>o.Status == "Đã thanh toán" || o.Status == "Đã hủy")
                        .Include(o => o.Customer)
                        .Include(o => o.OrderProductDetails)
                        .ToListAsync();
            ViewBag.Invoices = orders;
            return View();
        }
        [Route("SaleReport")]
        public async Task<IActionResult> SaleReport()
        {
            return View();
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Invoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
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

        // GET: Invoices/Edit/5
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
            return View(order);
        }

        // POST: Invoices/Edit/5
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

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        //-----------------HỦY ĐƠN HÀNG-------------------
        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id , string cancelReason)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                //Console.WriteLine($"------------------{id} {cancelReason}----------------");
                //_context.Orders.Remove(order);
                order.Status = "Đã hủy"; // thay đổi trạng thái của đơn hàng
                order.Note = cancelReason;
                _context.Update(order);
                await _context.SaveChangesAsync();

            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
