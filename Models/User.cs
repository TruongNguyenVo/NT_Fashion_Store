using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class User: IdentityUser
    {

        [Required(ErrorMessage = "Họ tên không được để trống.")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        public string? Address { get; set; }


		//[Required(ErrorMessage = "Ngày tháng năm sinh không được để trống.")]
		[DataType(DataType.Date)]
		public DateOnly? DateOfBrith { get; set; }

		[DefaultValue("Không xác định")]
		[RegularExpression(@"^(Nam|Nữ|Không xác định)$", ErrorMessage = "Chỉ nhận các giá trị Nam, Nữ, Hoặc Không xác định.")]
		public string? Gender { get; set; }

		//[Required(ErrorMessage = "Email không được để trống.")]
		//[EmailAddress]
		////[Remote(action: "IsEmailExists", controller: "Account")] // kiem tra email co trung khong
		//public string Email { get; set; }

		//[Required(ErrorMessage = "Mật khẩu không được để trống.")]
		//public string Password { get; set; }

		//[NotMapped]
		//[Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
		//public required string ConfirmPassword { get; set; }
		public Boolean IsDel { get; set; } = false;
        public Cart? Cart { get; set; } // mot user chi co 1 cart
        public List<Category>? Categories { get; set; } // 1 user quan ly nhieu category
        public List<Order>? Orders { get; set; } // 1 khach hang co nhieu order
        public List<PurchaseReport>? PurchaseReports { get; set; } //1 quan ly quan ly nhieu purchase report
    }
}
