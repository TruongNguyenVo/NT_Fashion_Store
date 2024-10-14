using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [ForeignKey(nameof(User.Id))]
        public int UserId { get; set; }
        public User? User { get; set; } // mot cart chi co 1 user

        public List<CartDetail>? CartDetails { get; set; } // cart chua nhieu detail

    }

}
