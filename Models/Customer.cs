using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace doan1_v1.Models
{
    public class Customer : User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone]
        public required string Phone { get; set; }


        [Required(ErrorMessage = "Ngày tháng năm sinh không được để trống.")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBrith { get; set; }

        [Required(ErrorMessage = "Giới tính không được để trống.")]
        [DefaultValue("Không xác định")]
        [RegularExpression(@"^(Nam|Nữ|Không xác định)$", ErrorMessage = "Chỉ nhận các giá trị Nam, Nữ, Hoặc Không xác định.")]
        public required string Gender { get; set; }
    }
}
