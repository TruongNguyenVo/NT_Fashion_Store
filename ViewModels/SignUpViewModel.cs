using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace doan1_v1.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Họ tên không được để trống.")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Ngày tháng năm sinh không được để trống.")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBrith { get; set; }

        public string Gender { get; set; }
        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress]
        //[Remote(action: "IsEmailExists", controller: "Account", ErrorMessage = "action run")] // kiem tra email co trung khong
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điên thoại không được để trống.")]
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống.")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        public string Address { get; set; }
    }
}
