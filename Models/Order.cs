namespace doan1_v1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateOrder { get; set; }
        public DateTime DatePredictSuccess { get; set; }
        public double DeliveryCost { get; set; }
        public double otherCost { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
        public int UserId { get; set; }
    }
}
