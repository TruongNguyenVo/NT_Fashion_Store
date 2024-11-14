using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using doan1_v1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace doan1_v1.Controllers
{
	[Authorize(Policy = "ManagerOnly")]
	public class ProductsController : Controller
    {
        private readonly NTFashionDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(NTFashionDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;


        }

        // GET: Products
        //public async Task<IActionResult> Index()
        //{
        //    var nTFashionDbContext = _context.Products.Include(p => p.Category);
        //    return View(await nTFashionDbContext.ToListAsync());
        //}
        public async Task<IActionResult> Index()
        {
            CategoriesController categories;
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            ViewBag.Products = products;

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            //tim hinh anh theo id
            var pathProductImages = await _context.ProductImages.Where(
                prId => prId.ProductId == id).
                Select(prId => new { Id = prId.Id, Name = prId.ImgURL }).ToListAsync();

            ViewBag.ProductImage = pathProductImages;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Description,Color,Dimension,Material,Quantity,Price,Productor,IsDel,CategoryId")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
        //    return View(product);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Color,Dimension,Material,Quantity,Price,Productor,IsDel,CategoryId")] Product product, List<IFormFile> files)
        {
            //lưu thông tin của sản phẩm trước
            
            if (ModelState.IsValid)
            {
                _context.Add(product); 
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));

                int rowsAffected = await _context.SaveChangesAsync();
                if (rowsAffected > 0)
                {                    
                    // Lưu thành công
                    //lưu thông tin hình ảnh sau
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "admin", "assets", "uploads"); // them duong dan upload trong wwwroot/assets/uploads

                    //tao duong dan chua anh neu duong dan khong co
                    if (!Directory.Exists(uploadsFolder))
                    {
                        
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    foreach (var file in files)
                    {
                        ProductImage image = new ProductImage();

                        // Đặt tên file là "abc" với phần mở rộng gốc
                        string fileExtension = Path.GetExtension(file.FileName); // lấy phần mở rộng của file gốc
                        string fileName = randomNameFile() + fileExtension; // Đặt tên file là random với phần mở rộng gốc

                        string fileSavePath = Path.Combine(uploadsFolder, fileName); // lấy đường dẫn của file đường dẫn tương đối
                        string relativePath = fileSavePath.Replace(_webHostEnvironment.WebRootPath, "");

                        //su dung fileStream de luu file
                        using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                        {
                
                            await file.CopyToAsync(stream);
                        }

                        // Xử lý đối tượng ProductImage
                        image.ProductId = product.Id; // Gán ID của sản phẩm cho ProductImage
                        image.ImgURL = relativePath; // Đặt đường dẫn ảnh cho ProductImage
                        _context.Add(image);
                        await _context.SaveChangesAsync();

                    }
                    return RedirectToAction(nameof(Index));
                }

            }
          

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            //tim hinh anh theo id
            var pathProductImages = await _context.ProductImages.Where(
                prId => prId.ProductId == product.Id).
                Select(prId => new { Id = prId.Id, Name = prId.ImgURL }).ToListAsync();

            ViewBag.ProductImage = pathProductImages;
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Color,Dimension,Material,Quantity,Price,Productor,IsDel,CategoryId")] Product product, List<IFormFile> files)
        {
            int effectRow; // cột thay đổi
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    effectRow = await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (effectRow > 0)
                {
                    // Lưu thành công
                    //lưu thông tin hình ảnh sau
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "admin", "assets", "uploads"); // them duong dan upload trong wwwroot/assets/uploads

                    //tao duong dan chua anh neu duong dan khong co
                    if (!Directory.Exists(uploadsFolder))
                    {

                        Directory.CreateDirectory(uploadsFolder);
                    }

                    foreach (var file in files)
                    {
                        ProductImage image = new ProductImage();

                        // Đặt tên file là "abc" với phần mở rộng gốc
                        string fileExtension = Path.GetExtension(file.FileName); // lấy phần mở rộng của file gốc
                        string fileName = randomNameFile() + fileExtension; // Đặt tên file là random với phần mở rộng gốc

                        string fileSavePath = Path.Combine(uploadsFolder, fileName); // lấy đường dẫn của file đường dẫn tương đối
                        string relativePath = fileSavePath.Replace(_webHostEnvironment.WebRootPath, "");

                        //su dung fileStream de luu file
                        using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                        {

                            await file.CopyToAsync(stream);
                        }

                        // Xử lý đối tượng ProductImage
                        image.ProductId = product.Id; // Gán ID của sản phẩm cho ProductImage
                        image.ImgURL = relativePath; // Đặt đường dẫn ảnh cho ProductImage
                        _context.Add(image);
                        await _context.SaveChangesAsync();

                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product != null)
            {
                //_context.Products.Remove(product);
                product.IsDel = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
        private string randomNameFile()
        {

            return Guid.NewGuid().ToString(); //tạo ra một chuỗi ngẫu nhiên không trùng lặp.
        }
    }
}
