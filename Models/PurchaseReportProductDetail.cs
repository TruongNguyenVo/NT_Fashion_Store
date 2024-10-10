using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class PurchaseReportProductDetail
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Product.Id))]
        public int ProductId { get; set; }
        public Product Product { get; set; } // mot detail chi chua 1 product

        [ForeignKey(nameof(PurchaseReport.Id))] // lien ket voi bang purchase
        public int PurchaseReportId { get; set; }
        public PurchaseReport PurchaseReport { get; set; } // 1 detail chi chua 1 purchase report

        public int Quantity { get; set; }
        public double PricePurchase { get; set; }
    }
}
