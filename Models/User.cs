using System.ComponentModel.DataAnnotations;

namespace doan1_v1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Họ tên không được để trống.")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress]
        //[Remote(action: "IsEmailExists", controller: "Account")] // kiem tra email co trung khong
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        public string Password { get; set; }
        public Boolean IsDel { get; set; } = false;
        public Cart? Cart { get; set; } // mot user chi co 1 cart
        public List<Category>? Categories { get; set; } // 1 user quan ly nhieu category
        public List<Order>? Orders { get; set; } // 1 khach hang co nhieu order
        public List<PurchaseReport>? PurchaseReports { get; set; } //1 quan ly quan ly nhieu purchase report
    }
}
