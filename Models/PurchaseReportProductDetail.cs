using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class PurchaseReportProductDetail
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Product.Id))]
        public int? ProductId { get; set; }
        public Product? Product { get; set; } // mot detail chi chua 1 product

        [ForeignKey(nameof(PurchaseReport.Id))] // lien ket voi bang purchase
        public int? PurchaseReportId { get; set; }
        public PurchaseReport? PurchaseReport { get; set; } // 1 detail chi chua 1 purchase report

        [Range(0,Int64.MaxValue,ErrorMessage ="Số lượng không được nhỏ hơn 0.")]
        [Required(ErrorMessage = "Số lương là bắt buộc.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá không được nhỏ hơn 0.")]
        public double PricePurchase { get; set; }
    }
}
