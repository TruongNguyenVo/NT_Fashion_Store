using System.ComponentModel.DataAnnotations.Schema;

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


        [ForeignKey(nameof(User.Id))] // khoa ngoai lien ket voi bang User
        public int AdminId { get; set; }

        [ForeignKey(nameof(User.Id))] // khoa ngoai lien ket voi bang User
        public int Customer { get; set; }
        public User User { get; set; } // order chi co 1 khach hang




    }
}
