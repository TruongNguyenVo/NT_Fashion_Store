using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class PurchaseReport
    {
        public int Id { get; set; }
        public string CodePurchaseReport { get; set; }
        public DateTime DatePurchase { get; set; }
        public double TotalPrice { get; set; }
        public string? Note { get; set; }
        [ForeignKey(nameof(Supplier.Id))] // lien ket voi bang Supplier
        public int SupplierId  { get; set; }
        public Supplier Supplier { get; set; } //mot purchase report chi thuoc 1 supplier

        [ForeignKey(nameof(User.Id))] // lien ket voi bang User
        public int UserId { get; set; }
        public User User { get; set; } // mot purchase report chi thuoc 1 quan ly

        public List<PurchaseReportProductDetail> PurchaseReportProductDetails { get; set; } // 1 purchase report chua nhieu detail
    }
}
