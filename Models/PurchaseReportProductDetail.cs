namespace doan1_v1.Models
{
    public class PurchaseReportProductDetail
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int PurchaseReportId { get; set; }
        public int Quantity { get; set; }
        public double PricePurchase { get; set; }
    }
}
