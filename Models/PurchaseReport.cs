namespace doan1_v1.Models
{
    public class PurchaseReport
    {
        public int Id { get; set; }
        public string CodePurchaseReport { get; set; }
        public DateTime DatePurchase { get; set; }
        public double TotalPrice { get; set; }
        public string? Note { get; set; }
        public int SupplierId  { get; set; }
        public int UserId { get; set; }

    }
}
