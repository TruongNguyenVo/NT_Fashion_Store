using doan1_v1.Helpers;
using doan1_v1.Models;
using doan1_v1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NT_Fashion_Store.ViewModels;
using SQLitePCL;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using VNPayPackage.Enums;

namespace doan1_v1.Controllers
{
    public class HomeController : Controller
    {
		//private string userId = "1fa6cac2-4e92-4206-8e54-fcf50fbbefa1"; //id test

		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly NTFashionDbContext _context;
		public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, NTFashionDbContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_context = context;
		}
		//lay cart cua user
        private async Task<List<CartDetail>> cartOfUser(string userId)
        {
			List<CartDetail> detailCarts = null;
			if (userId != null)
			{
				// tim kiem cart theo ten nguoi dung
				var cart = await _context.Carts
										   .Where(c => c.UserId == userId).FirstAsync();
				if (cart != null)
				{
					//tìm chi tiết sản phẩm
					detailCarts = await _context.CartDetails
												.Where(d => d.CartId == cart.Id)
												.Include(d => d.Product)
												.ThenInclude(p => p.ProductImages)
												.ToListAsync();
					if (detailCarts.Any())
					{

						//tính tổng tiền của giỏ hàng
						double totalPrice = 0;
						foreach (var detail in detailCarts)
						{
							int quantity = detail.Quantity;
							double price = (double)detail.Product.Price;
							totalPrice += quantity * price;
						}
					}
				}
				//            foreach(var detail in detailCarts)
				//            {
				//                Console.WriteLine(detail.Product.Name);
				//	Console.WriteLine(detail.Quantity);

				//}
			}

			return detailCarts;
		}
        [Route("")]
        public async Task<IActionResult> Index()
        {

			Console.WriteLine("-----------------------------------------");
            //viet de lay 8 san pham (4 quan, 4 ao)
			

            //ao
            var productAos = await _context.Products
                                            .Where(p => p.Category.ParentCategory.Name == "Áo" && p.IsDel == false && p.Quantity > 0 && p.ProductImages.Count > 0)
                                            .Include(p=> p.ProductImages)
                                            .Take(4)
                                            .ToListAsync();
            
			//quan
            var productQuans = await _context.Products
                                            .Where(p => p.Category.ParentCategory.Name == "Quần" && p.IsDel == false && p.Quantity > 0 && p.ProductImages.Count > 0)
                                            .Include(p=> p.ProductImages)
                                            .Take(4)
                                            .ToListAsync();

			//lay 3 productId trong bang orderdetail co xuat hien nhieu nhat
			var topProductIds = _context.OrderProductDetails
			.GroupBy(od => od.ProductId)
			.Select(g => new
			{
				ProductId = g.Key,
				Count = g.Count()
			})
			.OrderByDescending(g => g.Count)
			.Take(4)
			.Select(x => x.ProductId)
			.ToList();
			if (topProductIds != null)
			{
				var topProducts = await _context.Products.
										Where(p => topProductIds.Contains(p.Id) && p.IsDel == false && p.Quantity > 0 && p.ProductImages.Count > 0)
										.Include(p => p.ProductImages)
											.ToListAsync();
				ViewBag.Top4Seller = topProducts;
			}
			ViewBag.ProductAos = productAos;
            ViewBag.ProductQuans = productQuans;

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Gets the user's unique identifier (usually the ID from your user table).
            if(userId != null) {
                var detailCart = await cartOfUser(userId);

				ViewBag.cartOfUser = detailCart; // lay danh sach cac mon hang cua nguoi dung - undone
			}
			return View();
        }
        //hàm dùng để tìm kiếm sản phẩm
        [Route("Seach/{query?}")]
        public async Task<IActionResult> Search(string? query, int pageNumber = 1)
        {

			//lay 3 productId trong bang orderdetail co xuat hien nhieu nhat
			var topProductIds = _context.OrderProductDetails
			.GroupBy(od => od.ProductId)
			.Select(g => new
			{
				ProductId = g.Key,
				Count = g.Count()
			})
			.OrderByDescending(g => g.Count)
			.Take(3)
			.Select(x => x.ProductId)
			.ToList();
			if (topProductIds != null)
			{
				var topProducts = await _context.Products.
										Where(p => topProductIds.Contains(p.Id) && p.IsDel == false && p.Quantity > 0 && p.ProductImages.Count > 0)
										.Include(p => p.ProductImages)
											.ToListAsync();
				ViewBag.Top3Seller = topProducts;
			}





			//tim tat ca san pham theo query
			//Console.WriteLine($"--------------{query}----------------");

			//danh muc san pham
			var categories = await _context.Categories
								.Where(c => c.ParentId != null)
								.ToListAsync();
			ViewBag.categories = categories;

			var products = await PaginatedList<Product>.CreateAsync(_context.Products
	            .Where(p => p.IsDel == false && p.Name.Contains(query))
				.Include(p => p.ProductImages), pageNumber, 8); // phan trang moi 8 san pham 
			ViewBag.products = products;
			//foreach(var product in products)
			//{
			//    Console.WriteLine(product.Name);
			//}
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Gets the user's unique identifier (usually the ID from your user table).
			if (userId != null)
			{
				var detailCart = await cartOfUser(userId);

				ViewBag.cartOfUser = detailCart; // lay danh sach cac mon hang cua nguoi dung - undone
			}
			return View("Search", products);
        }
        [Route("Shop/{id?}")]
        public async Task<IActionResult> Shop(int? id, int pageNumber=1)
		{
			//lay 3 productId trong bang orderdetail co xuat hien nhieu nhat
			var topProductIds = _context.OrderProductDetails
			.GroupBy(od => od.ProductId)
			.Select(g => new
			{
				ProductId = g.Key,
				Count = g.Count()
			})
			.OrderByDescending(g => g.Count)
			.Take(3)
			.Select(x => x.ProductId)
			.ToList();
			if (topProductIds != null)
			{
				var topProducts = await _context.Products.
										Where(p => topProductIds.Contains(p.Id) && p.IsDel == false)
										.Include(p => p.ProductImages)
											.ToListAsync();
				ViewBag.Top3Seller = topProducts;
			}



			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Gets the user's unique identifier (usually the ID from your user table).
			if (userId != null)
			{
				var detailCart = await cartOfUser(userId);

				ViewBag.cartOfUser = detailCart; // lay danh sach cac mon hang cua nguoi dung - undone
			}

			//danh muc san pham
			var categories = await _context.Categories
								.Where(c => c.ParentId != null && c.IsDel == false)
								.ToListAsync();
			ViewBag.categories = categories;
			// lay tat ca san pham
			if (id == null)
            {
                //var products = await _context.Products
                //	.Where(p => p.IsDel == false)
                //                .Include(p=> p.ProductImages)
                //	.ToListAsync();
                var products = await PaginatedList<Product>.CreateAsync(_context.Products
                    .Where(p => p.IsDel == false && p.Quantity > 0 && p.ProductImages.Count > 0 )
                                .Include(p => p.ProductImages), pageNumber, 9); // phan trang moi 8 san pham 
                ViewBag.products = products;
				foreach(var product in products)
				{
                    Console.WriteLine($"-------------{product.Name}");
				}
				return View();
            }
            //lay san pham theo category
            else
            {
				var products = await PaginatedList<Product>.CreateAsync(_context.Products
	                                                        .Where(p => p.CategoryId == id && p.IsDel == false && p.Quantity > 0 && p.ProductImages.Count > 0)
															.Include(p => p.ProductImages), pageNumber, 9); // phan trang moi 8 san pham 
				ViewBag.products = products;
                return View();
            }
			
		}

		[Authorize(Policy = "CustomerOnly")]
		[Route("Order")]
        public async Task<IActionResult> Order()
        {
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Gets the user's unique identifier (usually the ID from your user table).
			if (userId != null)
			{
				var detailCart = await cartOfUser(userId);

				ViewBag.cartOfUser = detailCart; // lay danh sach cac mon hang cua nguoi dung - undone
			}

			var user = await _userManager.FindByIdAsync(userId);
            //Console.WriteLine($"--------------------{user?.FullName} | {user?.UserName}------------");

			var orders = await _context.Orders
                         .Where(o=> o.CustomerId == userId)
						.Include(o => o.OrderProductDetails)
						.ToListAsync();
			ViewBag.Orders = orders;
			return View();
        }
		[Authorize(Policy = "CustomerOnly")]
		[Route("Invoice")]
		public async Task<IActionResult> Invoice(int orderId, string title)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Gets the user's unique identifier (usually the ID from your user table).
			if (userId != null)
			{
				var detailCart = await cartOfUser(userId);

				ViewBag.cartOfUser = detailCart; // lay danh sach cac mon hang cua nguoi dung - undone
			}

			//Console.WriteLine($"-------------------{orderId}---------------------");
			var order = await _context.Orders
                    .Where(o => o.Id == orderId)
                    .Include(o => o.Customer)
                    .Include(o => o.OrderProductDetails) // lay danh sach chi tiet order trong order
                        .ThenInclude(op => op.Product) // lay thong tin cua product trong danh sach chi tiet
                            .ThenInclude(opc => opc.Category) // lay ten category trong product
                    .FirstOrDefaultAsync();
            if (order != null)
            {
                ViewBag.Invoice = order;
                //ViewData["title"] = title;
            }
            
			return View();
		}
		[Route("product/detail/{id:int}")]
        public async Task<IActionResult> Detail(int id)
		{
			//lay 3 productId trong bang orderdetail co xuat hien nhieu nhat
			var topProductIds = _context.OrderProductDetails
			.GroupBy(od => od.ProductId)
			.Select(g => new
			{
				ProductId = g.Key,
				Count = g.Count()
			})
			.OrderByDescending(g => g.Count)
			.Take(3)
			.Select(x => x.ProductId)
			.ToList();
			if (topProductIds != null)
			{
				var topProducts = await _context.Products.
										Where(p => topProductIds.Contains(p.Id))
											.ToListAsync();
				ViewBag.Top3Seller = topProducts;
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Gets the user's unique identifier (usually the ID from your user table).
			if (userId != null)
			{
				var detailCart = await cartOfUser(userId);

				ViewBag.cartOfUser = detailCart; // lay danh sach cac mon hang cua nguoi dung - undone
			}

			// san pham chinh
			var product = await _context.Products.
                           Where(p => p.Id == id)
                           .Include (p => p.ProductImages)
                           .Include(p=> p.Category)
						   .FirstOrDefaultAsync();
            ViewBag.product = product;
            //san pham tuong tu
            var relativeProducts = await _context.Products
                                    .Where(p=> p.CategoryId == product.CategoryId && p.IsDel == false)
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
		[Authorize(Policy = "CustomerOnly")]
		[Route("Checkout")]
        public async Task<IActionResult> Checkout()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Gets the user's unique identifier (usually the ID from your user table).
			if (userId != null)
			{
				var detailCart = await cartOfUser(userId);

				ViewBag.cartOfUser = detailCart; // lay danh sach cac mon hang cua nguoi dung - undone
			}


			var cartofUser = await _context.Carts.Where(c => c.UserId == userId).FirstOrDefaultAsync();

			//tìm chi tiết sản phẩm
			var details = await _context.CartDetails
										.Where(d => d.CartId == cartofUser.Id)
										.Include(d => d.Product)
										.ThenInclude(p => p.ProductImages)
										.ToListAsync();
			if (details.Any())
			{

				//tính tổng tiền của giỏ hàng
				double totalPrice = 0;
				foreach (var detail in details)
				{
					int quantity = detail.Quantity;
					double price = (double)detail.Product.Price;
					totalPrice += quantity * price;
				}
				ViewBag.details = details; //chi tiet gio hang
				ViewBag.totalPrice = totalPrice; // tong tien gio hang
                var cart = await _context.Carts.Where(c => c.Id == cartofUser.Id).Include(p=>p.User).FirstOrDefaultAsync();
                ViewBag.cart = cart;
			}

			return View();
		}
		[Authorize(Policy = "CustomerOnly")]
		//hàm dùng xác nhận đơn hàng
		public async Task<IActionResult> confirmCheckout(double deliveryCost, string paymentMethod)
        {
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
			if (userId != null)
			{
				var detailCart = await cartOfUser(userId);

				ViewBag.cartOfUser = detailCart; // lay danh sach cac mon hang cua nguoi dung - undone
			}
			Console.WriteLine($"payment method: {paymentMethod}");
			Console.WriteLine();


			var cartofUser = await _context.Carts.Where(c => c.UserId == userId).FirstOrDefaultAsync();

			//khi đặt hàng thì chuyển chi tiết giỏ hàng vào chi tiết order và xóa hết chi tiết giỏ hàng của cart đó
			//Console.WriteLine($"Vao post roi");
			//Console.WriteLine($"---------------------{customerId}------------------");
			//tao order
			Order order = new Order
			{
				Status = "Đã đặt hàng", /* gán giá trị phù hợp cho Status */
				PaymentMethod = paymentMethod,

			};
            order.DateOrder = DateOnly.FromDateTime(DateTime.Now);
            order.DeliveryCost = deliveryCost;
            order.CustomerId = userId;

            _context.Add(order);

            int rowsEffect = await _context.SaveChangesAsync();
            if(rowsEffect > 0) {
                //tao chi tiet order
                int orderId = order.Id;
                //tim cart de chuyen du lieu
                ////tim chi tiet cart dua vao cartId
                var detailCarts = await _context.CartDetails
                                        .Where(d => d.CartId == cartofUser.Id)
                                        .Include(d=>d.Product)
                                        .ToListAsync();

                ////chuyen tat ca cac chi tiet vao chi tiet order
                foreach(var detailCart in detailCarts)
                {
                    OrderProductDetail detailOder = new OrderProductDetail();
                    detailOder.Quantity = detailCart.Quantity;
                    detailOder.PriceSale = (double)detailCart.Product.Price;
                    detailOder.ProductId = detailCart.ProductId;
                    detailOder.OrderId = orderId;

					//tru quantity cua product
					var product = _context.Products
										.Where(d => d.Id == detailCart.ProductId).FirstOrDefault();
					product.Quantity = product.Quantity - detailCart.Quantity;
					_context.Products.Update(product);
					await _context.SaveChangesAsync();



                    _context.Add(detailOder);
                    await _context.SaveChangesAsync();
                }
                ////xoa tat ca cac chi tiet trong chi tiet cart
                foreach(var detailCart in detailCarts)
                {
                    _context.CartDetails.Remove(detailCart);
                    await _context.SaveChangesAsync();
                }
			}
			
			if (paymentMethod == "vn_pay")
			{
				double totalPrice = deliveryCost;
				string description = "";

				var listOrderDetail = _context.OrderProductDetails.Where(d => d.OrderId == order.Id).ToList();
				foreach(var orderDetail in listOrderDetail)
				{
					totalPrice = totalPrice + orderDetail.PriceSale;
					description = description + orderDetail.Product?.Name + "--" + orderDetail.Quantity + "--" + orderDetail.PriceSale + ".\n";
				}

				Console.WriteLine(totalPrice);
				Console.WriteLine(description);
				Console.WriteLine();

				PaymentInformationModel model = new PaymentInformationModel();
				model.OrderType = "Online";
				model.Amount = totalPrice;
				model.OrderId = order.Id.ToString();
				model.OrderDescription = description;
				model.Name = "Thanh toán online ở NTFashion";
				return RedirectToAction("CreatePaymentUrl", "Payment", model);
			}
			if (paymentMethod == "paypal")
			{
				PaypalRequest paypalRequest = new PaypalRequest();
				paypalRequest.guid = "12345";         // Mã định danh giao dịch ngẫu nhiên
				paypalRequest.PayerID = "ABCD12345";   // ID của người thanh toán do PayPal cung cấp
				paypalRequest.Cancel = null;         // Người dùng không hủy giao dịch


				Console.WriteLine("Thannhhhhhhhhhh toannnnnnnnnnn banggggggg paypalllllllllll");
				return RedirectToAction("PaymentWithPaypal", "Paypal", paypalRequest);
			}
			Console.WriteLine();
			// chuyển đến trang order
			return RedirectToAction("Order", "Home");
		}
		[Authorize(Policy = "CustomerOnly")]
		[Route("Cart")]
        public async Task<IActionResult> Cart()
        {

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Gets the user's unique identifier (usually the ID from your user table).
			if (userId != null)
			{
				var detailCart = await cartOfUser(userId);

				ViewBag.cartOfUser = detailCart; // lay danh sach cac mon hang cua nguoi dung - undone
			}

			var cart = await _context.Carts
                                    .Where(c => c.UserId == userId)
                                    .FirstOrDefaultAsync();
            if (cart != null)
			{
                //tìm chi tiết sản phẩm
				var details = await _context.CartDetails
                                            .Where(d => d.CartId == cart.Id)
                                            .Include(d => d.Product)
                                            .ThenInclude(p => p.ProductImages)
                                            .ToListAsync();
                if (details.Any())
                {
                    
                    //tính tổng tiền của giỏ hàng
                    double totalPrice = 0;
                    foreach(var detail in details)
                    {
                        int quantity = detail.Quantity;
                        double price = (double)detail.Product.Price;
                        totalPrice += quantity * price;
                    }
					ViewBag.details = details; //chi tiet gio hang
					ViewBag.totalPrice = totalPrice; // tong tien gio hang
                }
                else
                {
                    ViewBag.details = null;
                }
			}
            else
            {
				ViewBag.details = null;
				
			}
			return View();

		}

		// hàm thêm vào giỏ hàng cần số lượng sản phẩm, id sản phẩm, id của user đã đăng nhập
		[Authorize(Policy = "CustomerOnly")]
		public async Task<IActionResult> addToCart(int quantity, int productId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Gets the user's unique identifier (usually the ID from your user table).

																		 //tạo chi tiết giỏ hàng

			//đã có chi tiết giỏ hàng

			//tìm kiếm cart của một khách hàng dựa vào userId
			var cart = await _context.Carts
                            .Where(p => p.UserId == userId)
                            .FirstOrDefaultAsync();

			//tim kiem neu co trong detail thi cong them quantity, khong co thi tao moi
			var detailsCart = await _context.CartDetails
											.Where(p => p.CartId == cart.Id)
											.ToListAsync();

			if(detailsCart == null)
			{
                //Console.WriteLine();
                //tạo một chi tiết giỏ hàng
                CartDetail detail = new CartDetail();
                detail.ProductId = productId;
                detail.Quantity = quantity;
                detail.CartId = cart.Id;
                _context.Add(detail);
                int rowsEffect = await _context.SaveChangesAsync();

                // về trang hiện tại (không di chuyển qua trang khác)
                return Redirect(Request.Headers["Referer"].ToString());

            }
			else
			{
                Console.WriteLine();
				if(detailsCart.Count > 0) { 
					foreach (var detailCart in detailsCart)
					{
						//neu product co trong detail
						if (detailCart.ProductId == productId)
						{
							detailCart.Quantity = detailCart.Quantity + quantity;
							await _context.SaveChangesAsync();
							// về trang hiện tại (không di chuyển qua trang khác)
							return Redirect(Request.Headers["Referer"].ToString()); // neu trung productId thi cộng quantity
						}

					}
					//nếu không trùng thì tạo cái mới
					//tạo một chi tiết giỏ hàng
					CartDetail detail = new CartDetail();
					detail.ProductId = productId;
					detail.Quantity = quantity;
					detail.CartId = cart.Id;
					_context.Add(detail);
					int rowsEffect = await _context.SaveChangesAsync();
					// về trang hiện tại (không di chuyển qua trang khác)
					return Redirect(Request.Headers["Referer"].ToString());
				}
				else { 
                //tạo một chi tiết giỏ hàng
                CartDetail detail = new CartDetail();
                    detail.ProductId = productId;
                    detail.Quantity = quantity;
                    detail.CartId = cart.Id;
                    _context.Add(detail);
                    int rowsEffect = await _context.SaveChangesAsync();
                }
                // về trang hiện tại (không di chuyển qua trang khác)
                return Redirect(Request.Headers["Referer"].ToString());
            }


            //if(rowsEffect > 0)
            //{
            //    Console.WriteLine("Da tao cartdetail");
            //}
            //else
            //{
            //    Console.WriteLine("loi roi");
            //}


		}

		//hàm xóa chi tiết trong giỏ hàng, nhận vào carid, id cua chi tiet
		[Authorize(Policy = "CustomerOnly")]
		public async Task<IActionResult> delInCart(int detailId)
        {
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Gets the user's unique identifier (usually the ID from your user table).

			//tạo chi tiết giỏ hàng

			//đã có chi tiết giỏ hàng

			//tìm kiếm cart của một khách hàng dựa vào userId
			var cart = await _context.Carts
							.Where(p => p.UserId == userId)
							.FirstOrDefaultAsync();

			//Console.WriteLine($"------------------{detailId} {cartId}--------------------");

			//tìm cartdetail dựa vào cartId và detailId
			var detailCart = await _context.CartDetails
                                            .Where(d=> d.CartId == cart.Id && d.Id == detailId)
                                            .FirstOrDefaultAsync();

            if(detailCart != null)
            {
                //Console.WriteLine("----------------co san pham------------------");
                _context.CartDetails.Remove(detailCart);
                //xoa ra khoi gio hang luon
                await _context.SaveChangesAsync();
            }
			// về trang hiện tại (không di chuyển qua trang khác)
			return Redirect(Request.Headers["Referer"].ToString());
		}

		//hàm chỉnh sửa số lượng sản phẩm trong giỏ hàng, nhận vào cartId, id của chi tiết và quantity
		[Authorize(Policy = "CustomerOnly")]
		public async Task<IActionResult> changeQuantity(int detailId, int quantity)
        {
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //Gets the user's unique identifier (usually the ID from your user table).


			var cartofUser = await _context.Carts.Where(c => c.UserId == userId).FirstOrDefaultAsync();



			//Console.WriteLine($"----------------{detailId} {cartId} {quantity}------------");
			//đã có các tham số rồi
			//tìm cartdetail dựa vào cartId và detailId
			var detailCart = await _context.CartDetails
                                            .Where(d => d.CartId == cartofUser.Id && d.Id == detailId)
                                            .FirstOrDefaultAsync();


            if (detailCart != null)
            {
                detailCart.Quantity = quantity;
                _context.Update(detailCart);
				await _context.SaveChangesAsync();
			}

            // về trang hiện tại (không di chuyển qua trang khác)
            return Redirect(Request.Headers["Referer"].ToString());
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
