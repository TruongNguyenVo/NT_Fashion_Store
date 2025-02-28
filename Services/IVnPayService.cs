using doan1_v1;
using doan1_v1.ViewModels;

namespace doan1_v1.Services;
public interface IVnPayService
{
	string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
	PaymentResponseModel PaymentExecute(IQueryCollection collections);
}