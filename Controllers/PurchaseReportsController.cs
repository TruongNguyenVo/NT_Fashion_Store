using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using doan1_v1.Models;

namespace doan1_v1.Controllers
{
    public class PurchaseReportsController : Controller
    {
        private readonly NTFashionDbContext _context;

        public PurchaseReportsController(NTFashionDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseReports
        //public async Task<IActionResult> Index()
        //{
        //    var nTFashionDbContext = _context.PurchaseReports.Include(p => p.Supplier).Include(p => p.User);
        //    return View(await nTFashionDbContext.ToListAsync());
        //}
        public async Task<IActionResult> Index()
        {
            var purchaseReports = await _context.PurchaseReports.Include(p => p.Supplier).Include(p => p.User).ToListAsync();
            ViewBag.PurchaseReports = purchaseReports;
            return View(purchaseReports);
        }

        // GET: PurchaseReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseReport = await _context.PurchaseReports
                .Include(p => p.Supplier)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseReport == null)
            {
                return NotFound();
            }

            return View(purchaseReport);
        }

        // GET: PurchaseReports/Create
        public async Task<IActionResult> Create()
        {
            //lay danh sach danh muc san pham
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: PurchaseReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodePurchaseReport,DatePurchase,TotalPrice,Note,Status,SupplierId,UserId")] PurchaseReport purchaseReport, List<string> name_products, List<int> categoryIds, List<string> colors, List<string> dimensions, List<string> materials, List<string> productors, List<int> quantitys, List<double> prices)
        {
            //thêm thông tin của phiếu nhập
            //thêm sản phẩm vào bảng sản phẩm
            //tìm nếu các thông tin sản phẩm trùng thì lấy cái id thêm vào chi tiết phiếu nhập, không thì thêm cái mới
            //thêm thông tin vào bảng chi tiết phiếu nhập

            int purchaseReportId = 0;
            if (ModelState.IsValid)
            {
                
                _context.Add(purchaseReport);
                int affect = await _context.SaveChangesAsync();
                //done - có id của purchaseReport
                if (affect > 0)
                {
                    purchaseReportId = purchaseReport.Id;
                }
                //return RedirectToAction(nameof(Index));
            }


                for (int i = 0;
                i < name_products.Count;
                i++)
            {
                //tìm nếu các thông tin sản phẩm trùng
                var product = await _context.Products.FirstOrDefaultAsync(
                    pr => pr.Name == name_products[i]
                    && pr.CategoryId == categoryIds[i]
                    && pr.Color == colors[i]
                    && pr.Dimension == dimensions[i]
                    && pr.Material == materials[i]
                    && pr.Productor == productors[i]
                    //&& pr.Quantity == quantitys[i]
                    && pr.Price == prices[i]);
                int productId = 0; // bien luu id cua product
                // done - có id của product
                if (product != null)
                {
                    productId = product.Id; // có id của product

                }
                // done - có id của prouduct
                else
                {
                    // khong thi tao san pham moi trong bang san pham va bo cot price
                    Product new_product = new Product();
                    new_product.Name = name_products[i];
                    new_product.CategoryId = categoryIds[i];
                    new_product.Color = colors[i];
                    new_product.Dimension = dimensions[i];
                    new_product.Material = materials[i];
                    new_product.Productor = productors[i];

                    _context.Add(new_product);
                    //neu co thay doi
                    int affect = await _context.SaveChangesAsync();
                    if(affect != 0)
                    {
                        productId = new_product.Id; // có id của product
                    }
                }
                
                //thêm thông tin vào bảng chi tiết sản phẩm
                 
                    PurchaseReportProductDetail purchaseReportProductDetail = new PurchaseReportProductDetail();
                    purchaseReportProductDetail.ProductId = productId;
                    purchaseReportProductDetail.PurchaseReportId = purchaseReportId;
                    purchaseReportProductDetail.Quantity = quantitys[i];
                    purchaseReportProductDetail.PricePurchase = prices[i];

                    _context.Add(purchaseReportProductDetail);
                    await _context.SaveChangesAsync();

                

            }

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Address", purchaseReport.SupplierId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Address", purchaseReport.UserId);
            return View(purchaseReport);
        }

        // GET: PurchaseReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseReport = await _context.PurchaseReports.FindAsync(id);
            if (purchaseReport == null)
            {
                return NotFound();
            }

            //lấy tất cả các productid trong chi tiết phiếu nhập dựa vào id phiếu nhập
            // dựa vào id đó 

            var products = 0;


            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Address", purchaseReport.SupplierId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Address", purchaseReport.UserId);
            ViewData["Products"] = products;
            return View(purchaseReport);
        }

        // POST: PurchaseReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodePurchaseReport,DatePurchase,TotalPrice,Note,Status,SupplierId,UserId")] PurchaseReport purchaseReport)
        {
            if (id != purchaseReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseReportExists(purchaseReport.Id))
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
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Address", purchaseReport.SupplierId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Address", purchaseReport.UserId);
            return View(purchaseReport);
        }

        // GET: PurchaseReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseReport = await _context.PurchaseReports
                .Include(p => p.Supplier)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchaseReport == null)
            {
                return NotFound();
            }

            return View(purchaseReport);
        }

        // POST: PurchaseReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseReport = await _context.PurchaseReports.FindAsync(id);
            if (purchaseReport != null)
            {
                _context.PurchaseReports.Remove(purchaseReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseReportExists(int id)
        {
            return _context.PurchaseReports.Any(e => e.Id == id);
        }
    }
}
