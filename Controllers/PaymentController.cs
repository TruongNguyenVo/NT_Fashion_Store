using doan1_v1.Models;
using doan1_v1.Services;
using doan1_v1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace doan1_v1.Controllers
{
	[Route("[controller]")]
	public class PaymentController : ControllerBase
	{
		private readonly IVnPayService _vnPayService;
		private readonly IConfiguration _configuration;
		//private readonly OrderService _orderService;
		private readonly NTFashionDbContext _context;

		public PaymentController(IVnPayService vnPayService, IConfiguration configuration,
			NTFashionDbContext context)
		{
			_vnPayService = vnPayService;
			_configuration = configuration;
			_context = context;
			//_orderService = orderService;

		}

		public IActionResult CreatePaymentUrl(PaymentInformationModel model)
		{

			var paymentUrl = _vnPayService.CreatePaymentUrl(model, HttpContext);
			//Console.WriteLine($"{model.Name} - {model.OrderId} - {model.OrderType} - {model.OrderDescription} - {model.Amount}");
			//Console.WriteLine(paymentUrl);
			return Redirect(paymentUrl);
		}

		[HttpGet("Callback")]
		public async Task<IActionResult> PaymentCallback()
		{
			var response = _vnPayService.PaymentExecute(Request.Query);

			// Xác định trạng thái thanh toán
			var orderId = int.Parse(response.OrderId);
			var success = response.Success;

			//// Cập nhật trạng thái đơn hàng
			//if (success)
			//{
			//	await _orderService.UpdateOrderStatusAsync(orderId, "Paid", "Success");
			//}
			//else
			//{
			//	await _orderService.UpdateOrderStatusAsync(orderId, "Unpaid", "Failed");
			//}
			if (success)
			{
				var order = _context.Orders.Where(o => o.Id == orderId).FirstOrDefault();
				order.Status = "Đã thanh toán";
				await _context.SaveChangesAsync();

			}


			// Redirect về URL callback client
			var clientReturnUrl = _configuration["PaymentCallBack:ReturnUrl"];
			var finalRedirectUrl = $"{clientReturnUrl}?success={success}&orderId={response.OrderId}";

			Console.Write(finalRedirectUrl);
			Console.WriteLine();
			return RedirectToAction("Order","Home");
		}
	}
}