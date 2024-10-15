using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class PurchaseReport
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã hóa đơn của phiếu nhập kho là bắt buộc.")]
        public string CodePurchaseReport { get; set; }

        [Required(ErrorMessage = "Ngày nhập kho là bắt buộc.")]
        [DataType(DataType.Date, ErrorMessage ="Vui lòng nhập đúng chuẩn.")]
        public DateTime DatePurchase { get; set; }

        [Required(ErrorMessage = "Tổng tiền là bắt buộc.")]
        public double TotalPrice { get; set; }

        public string? Note { get; set; }
        public string Status { get; set; }
        [ForeignKey(nameof(Supplier.Id))] // lien ket voi bang Supplier
        public int SupplierId  { get; set; }
        public Supplier? Supplier { get; set; } //mot purchase report chi thuoc 1 supplier

        [ForeignKey(nameof(User.Id))] // lien ket voi bang User
        public int UserId { get; set; }
        public User? User { get; set; } // mot purchase report chi thuoc 1 quan ly

        public List<PurchaseReportProductDetail>? PurchaseReportProductDetails { get; set; } // 1 purchase report chua nhieu detail
    }
}
