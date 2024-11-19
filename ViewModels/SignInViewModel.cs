using System.ComponentModel.DataAnnotations;

namespace doan1_v1.ViewModels
{
	public class SignInViewModel
	{
		[Required(ErrorMessage ="Tên đăng nhập không được để trống")]
		public required string Username {  get; set; }
		[Required(ErrorMessage ="Mật khẩu không được để trống")]
		public required string Password { get; set; }
	}
}
