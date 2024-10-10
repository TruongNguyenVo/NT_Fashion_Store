namespace doan1_v1.Models
{
    public class OrderProductDetail
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double PriceSale { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
    }
}
