using System.ComponentModel.DataAnnotations;

namespace doan1_v1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBrith { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }

        public Cart Cart { get; set; } // mot user chi co 1 cart
        public List<Category>? Categories { get; set; } // 1 user quan ly nhieu category
        public List<Order> Orders { get; set; } // 1 khach hang co nhieu order
        public List<PurchaseReport> PurchaseReports { get; set; } //1 quan ly quan ly nhieu purchase report
    }
}
