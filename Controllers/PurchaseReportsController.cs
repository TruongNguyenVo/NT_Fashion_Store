using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using doan1_v1.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace doan1_v1.Controllers
{
	[Authorize(Policy = "ManagerOnly")]
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
                                .Include(pchase => pchase.PurchaseReportProductDetails) // them chi tiet phieu nhap
                                .ThenInclude(prchDetail => prchDetail.Product) // them product
                                .ThenInclude(cate => cate.Category)
                                 .FirstOrDefaultAsync(pchase => pchase.Id == id);
            // Lấy danh sách Category
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Address", purchaseReport.SupplierId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Address", purchaseReport.UserId);

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
                    //new_product.Dimension = dimensions[i];
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
            return RedirectToAction(nameof(Index));

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

            var purchaseReport = await _context.PurchaseReports
                                .Include(pchase =>pchase.PurchaseReportProductDetails) // them chi tiet phieu nhap
                                .ThenInclude(prchDetail => prchDetail.Product) // them product
                                .ThenInclude(cate =>cate.Category)
                                 .FirstOrDefaultAsync(pchase =>pchase.Id == id);
            // Lấy danh sách Category
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;


            if (purchaseReport == null)
            {
                return NotFound();
            }

            //lấy tất cả các productid trong chi tiết phiếu nhập dựa vào id phiếu nhập
            // dựa vào id đó 



            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", purchaseReport.SupplierId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Address", purchaseReport.UserId);
            return View(purchaseReport);
        }

        // POST: PurchaseReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodePurchaseReport,DatePurchase,OtherCost,Note,IsUpdate,SupplierId,UserId")] PurchaseReport purchaseReport, List<string> name_products, List<int> categoryIds, List<string> colors, List<string> dimensions, List<string> materials, List<string> productors, List<int>? quantitys, List<double>? prices)
        {
            if (id != purchaseReport.Id)
            {
                return NotFound();
            }
            //cập nhật thông tin của phiếu nhập (code, date, otherprice, note, isupdate, supplier)
            //xóa tất cả các chi tiết của chi tiết phiếu nhập
            //kiểm tra tất cả các product có productId trong chi tiết phiếu nhập => nếu thiếu cột price và quantity thì xóa
            //tạo chi tiết mới như tạo mới giống hàm create

            //cập nhật thông tin của phiếu nhập (code, date, otherprice, note, isupdate, supplier)
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseReport);
                    int affect = await _context.SaveChangesAsync();
                    //done - có id của purchaseReport
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

                //return RedirectToAction(nameof(Index));
            }
            else
            {
                // Duyệt qua các lỗi trong ModelState
                foreach (var entry in ModelState)
                {
                    var key = entry.Key; // Tên trường
                    var errors = entry.Value.Errors; // Danh sách lỗi cho trường đó

                    foreach (var error in errors)
                    {
                        // Xử lý hoặc ghi log lỗi ở đây
                        Console.WriteLine($"Field: {key}, Error: {error.ErrorMessage}");
                    }
                }

                // Trả lại view cùng với lỗi hiển thị nếu dữ liệu không hợp lệ
                return View(purchaseReport);
            }
            //tìm danh sách tất cả các productId của phiếu nhập trong chi tiết phiếu nhập
            var productIds = await _context.PurchaseReportProductDetails.
                            Where(detail => detail.PurchaseReportId == purchaseReport.Id).
                            Select(detail => detail.ProductId).ToListAsync();

            //xóa tất cả các chi tiết của chi tiết phiếu nhập
            var purchaseReportDetails = await _context.PurchaseReportProductDetails
                            .Where(detail => detail.PurchaseReportId == purchaseReport.Id)
                            .ToListAsync(); // nguyên cái danh sách chi tiết ứng với id

            _context.PurchaseReportProductDetails.RemoveRange(purchaseReportDetails);
            await _context.SaveChangesAsync();

            // ----------------------------done-----------------------------------

            //kiểm tra tất cả các product có productId trong chi tiết phiếu nhập => nếu thiếu cột price và quantity thì xóa
            var removeProduct = await _context.Products
                               .Where(p => productIds.Contains(p.Id) && (p.Price == null))
                               .ToListAsync(); // tìm id để xóa mấy cái product bị thiếu cột

            // Xóa các sản phẩm không hợp lệ
            if (removeProduct.Any())
            {
                _context.Products.RemoveRange(removeProduct);
                await _context.SaveChangesAsync();
            }



            //return RedirectToAction(nameof(Index));
            //----------Xoa bang chi tiet san pham, xoa cac san pham ko dung yeu cau ----------------
            for (int i = 0; i < name_products.Count; i++)
            {
               
                if (name_products[i].IsNullOrEmpty())
                {
                    continue;
                }
                //tim san pham trung
                var product = await _context.Products.FirstOrDefaultAsync(
                    pr => pr.Name == name_products[i]
                    && pr.CategoryId == categoryIds[i]
                    && pr.Color == colors[i]
                    && pr.Dimension == dimensions[i]
                    && pr.Material == materials[i]
                    && pr.Productor == productors[i]
                    && pr.Price == prices[i]);

                int productId = 0; // bien luu id cua product
                // done - có id của product
                if (product != null)
                {
                    productId = product.Id; // có id của product
                    Console.WriteLine("Co san pham");
                }
                // done - có id của prouduct
                else
                {
                    // khong thi tao san pham moi trong bang san pham va bo cot price
                    Product new_product = new Product();
                    new_product.Name = name_products[i];
                    new_product.CategoryId = categoryIds[i];
                    new_product.Color = colors[i];
                    //new_product.Dimension = dimensions[i];
                    new_product.Material = materials[i];
                    new_product.Productor = productors[i];

                    _context.Add(new_product);
                    //neu co thay doi
                    int affect = await _context.SaveChangesAsync();
                    if (affect != 0)
                    {
                        productId = new_product.Id; // có id của product
                    }
                }

                //thêm thông tin vào bảng chi tiết sản phẩm

                PurchaseReportProductDetail purchaseReportProductDetail = new PurchaseReportProductDetail();
                purchaseReportProductDetail.ProductId = productId;
                purchaseReportProductDetail.PurchaseReportId = purchaseReport.Id;
                purchaseReportProductDetail.Quantity = quantitys[i];
                purchaseReportProductDetail.PricePurchase = prices[i];

                _context.Add(purchaseReportProductDetail);
                await _context.SaveChangesAsync();



            }

            return RedirectToAction(nameof(Index));
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
                //_context.PurchaseReports.Remove(purchaseReport);
                purchaseReport.IsDel = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseReportExists(int id)
        {
            return _context.PurchaseReports.Any(e => e.Id == id);
        }
        // ham cap nhat so luong dua vao phieu nhap kho
        public async Task<IActionResult> UpdateQuantity(int id)
        {
           
            //tim tat ca product trong bang chi tiet
                //tìm danh sách tất cả các productId của phiếu nhập trong chi tiết phiếu nhập
            var productInDetails = await _context.PurchaseReportProductDetails.
                            Where(detail => detail.PurchaseReportId == id).
                            Select(detail => new
                            {
                                ProductId = detail.ProductId,
                                Quantity = detail.Quantity,
                                PricePurchase = detail.PricePurchase 
                            }).
                            ToListAsync();

            //lap qua cac product
            foreach (var productInDetail in productInDetails) { 
                var product = await _context.Products.FindAsync(productInDetail.ProductId);
                if (product != null)
                {
                    // co product, co product
                    //cap nhat so luong trong bang chi tiet vao bang product
                    product.Quantity = product.Quantity + productInDetail.Quantity;
                    //cap nhat gia trong bang chi tiet vao bang product
                    product.Price = productInDetail.PricePurchase;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }
                else
                { 
                    Console.WriteLine("Khong co san pham. Sai roi do"); 
                }
            }

            //cap nhat lại IsUpdate
            var puchaseReport = await _context.PurchaseReports.FindAsync(id);
            if (puchaseReport != null) {
                puchaseReport.IsUpdate = true;
                await _context.SaveChangesAsync();
            }
            //het chuyện
            return RedirectToAction(nameof(Index));
        }
    }
}
