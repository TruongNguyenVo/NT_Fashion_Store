using System.ComponentModel.DataAnnotations;

namespace doan1_v1.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên nhà cung cấp không được vượt quá 100 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Địa chỉ nhà cung cấp là bắt buộc.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Số điện thoại nhà cung cấp là bắt buộc.")]
        [Phone]
        public string Phone { get; set; }

        public List<PurchaseReport> PurchaseReports { get; set; } // 1 supplier co nhieu purchase report
    }
}
