using Azure.Core;
using Microsoft.AspNetCore.Mvc;
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

		public PaypalController(ILogger<PaypalController> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
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
					string baseURI = $"{Request.Scheme}://{Request.Host}/Payment/PaymentWithPaypal?";
					paypalRequest.guid = Convert.ToString((new Random()).Next(100000));
					var createdPayment = CreatePaymentWithPaypal(apiContext, $"{baseURI}guid={paypalRequest.guid}");
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
		private Payment CreatePaymentWithPaypal(APIContext apiContext, string redirectUrl)
		{
			var itemList = new ItemList() { items = new List<Item>() };

			itemList.items.Add(new Item()
			{
				name = "Product Name",
				currency = "USD",
				price = "10",
				quantity = "1",
				sku = "sku"
			});

			var payer = new Payer() { payment_method = "paypal" };
			var redirUrls = new RedirectUrls()
			{
				cancel_url = $"{redirectUrl}&Cancel=true",
				return_url = redirectUrl
			};

			var details = new Details()
			{
				tax = "1",
				shipping = "1",
				subtotal = "10"
			};

			var amount = new Amount()
			{
				currency = "USD",
				total = "12",
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
