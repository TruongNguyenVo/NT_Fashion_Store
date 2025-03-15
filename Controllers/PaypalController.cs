using Azure.Core;
using doan1_v1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NT_Fashion_Store.Helpers;
using NT_Fashion_Store.ViewModels;
using PayPal.Api;

namespace NT_Fashion_Store.Controllers
{
	[Route("[controller]")]
	public class PaypalController : ControllerBase
	{
		private readonly ILogger<PaypalController> _logger;
		private readonly IConfiguration _configuration;
		private readonly NTFashionDbContext _context;
		public PaypalController(ILogger<PaypalController> logger, IConfiguration configuration, NTFashionDbContext context)
		{
			_logger = logger;
			_configuration = configuration;
			_context = context;
		}
		public ActionResult PaymentWithPaypal(PaypalRequest paypalRequest)
		{
			Console.WriteLine("PaymentWithPaypalllllllllllllllllllllllllllllllllllll");
			var clientId = _configuration.GetValue<string>("PayPal:ClientId");
			var clientSecret = _configuration.GetValue<string>("PayPal:Secret");
			var mode = _configuration.GetValue<string>("PayPal:Mode");
			var apiContext = PaypalConfiguration.GetAPIContext(clientId, clientSecret, mode);

			Console.WriteLine($"{paypalRequest.PayerID} - {paypalRequest.guid} - {paypalRequest.Cancel}");
			Console.WriteLine();
			try
			{
				if (!string.IsNullOrEmpty(paypalRequest.PayerID))
				{
					string baseURI = $"{Request.Scheme}://{Request.Host}/Order?";
					paypalRequest.guid = Convert.ToString((new Random()).Next(100000));
					var createdPayment = CreatePaymentWithPaypal(apiContext, $"{baseURI}guid={paypalRequest.guid}", paypalRequest.OrderId);
					var links = createdPayment.links.GetEnumerator();
					string paypalRedirectUrl = null;

					while (links.MoveNext())
					{
						Links lnk = links.Current;
						if (lnk.rel.ToLower().Trim().Equals("approval_url"))
						{
							paypalRedirectUrl = lnk.href;
						}
					}

					HttpContext.Session.SetString($"payment_{paypalRequest.guid}", createdPayment.id);
					return Redirect(paypalRedirectUrl);
				}
				else
				{
					var paymentId = HttpContext.Session.GetString($"payment_{paypalRequest.guid}");
					var executedPayment = ExecutePayment(apiContext, paypalRequest.PayerID, paymentId);

					if (executedPayment.state.ToLower() != "approved")
					{
						return RedirectToAction("Checkout", "Home"); // thanh toan that bai
					}
					else
					{
						return RedirectToAction("Order", "Home"); // ✅ Thanh toán thành công
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during PayPal payment process.");
				return RedirectToAction("Checkout", "Home"); // thanh toan that bai
			}

			return RedirectToAction("Order", "Home"); // thanh toan thanh cong
		}
		private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
		{
			var paymentExecution = new PaymentExecution() { payer_id = payerId };
			var payment = new Payment() { id = paymentId };
			return payment.Execute(apiContext, paymentExecution);
		}
		private Payment CreatePaymentWithPaypal(APIContext apiContext, string redirectUrl, int orderId)
		{
			var order = _context.Orders.Find(orderId);
			if (order == null)
			{
				throw new Exception("Order not found");
			}
			double exchangeRate = 25000; // Giả sử 1 USD = 25,000 VND, bạn có thể cập nhật theo tỷ giá thực tế
			double deliveryCost = order.DeliveryCost /exchangeRate;
			string description = "";
			double totalPrice = 0;
			var listOrderDetail = _context.OrderProductDetails.Where(d => d.OrderId == order.Id)
				.Include(d => d.Product) // Load thêm thông tin Product
				.ToList();
			
			var itemList = new ItemList() { items = new List<Item>() };			
			
			foreach (var orderDetail in listOrderDetail)
			{
				
				double priceUSD = orderDetail.PriceSale / exchangeRate; // Chuyển đổi sang USD
				itemList.items.Add(new Item()
				{
					name = orderDetail.Product.Name,
					currency = "USD",
					price = priceUSD.ToString(),
					quantity = orderDetail?.Quantity.ToString(),
					sku = "sku"

				});
				totalPrice = totalPrice + priceUSD;
				description = description + orderDetail.Product.Name + "--" + orderDetail.Quantity + "--" + priceUSD + ".\n";
			}
			Console.WriteLine("Total Price: " + totalPrice);
			Console.WriteLine("Description: " + description);

			var payer = new Payer() { payment_method = "paypal" };
			var redirUrls = new RedirectUrls()
			{
				cancel_url = $"{redirectUrl}&Cancel=true",
				return_url = redirectUrl
			};

			var details = new Details()
			{
				tax = "0",
				shipping = deliveryCost.ToString(),
				subtotal = totalPrice.ToString(),
			};

			double total = totalPrice + deliveryCost;

			Console.WriteLine("Total: " + total);
			Console.WriteLine($"{details.tax} + {details.shipping} + {details.subtotal}");
			Console.WriteLine();
			var amount = new Amount()
			{
				currency = "USD",
				total = total.ToString(),
				//total = totalPriceUSD.ToString(),
				details = details
			};

			var transactionList = new List<Transaction>();
			transactionList.Add(new Transaction()
			{
				description = "Transaction description",
				invoice_number = Convert.ToString((new Random()).Next(100000)),
				amount = amount,
				item_list = itemList
			});

			var payment = new Payment()
			{
				intent = "sale",
				payer = payer,
				transactions = transactionList,
				redirect_urls = redirUrls
			};

			return payment.Create(apiContext);
		}

	}
}
