using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class CartDetail
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Số lượng sản phẩm là bắt buộc.")]
        [Range(0, Int64.MaxValue, ErrorMessage = "Số lượng sản phẩm không được nhỏ hơn 0.")]
        public int Quantity { get; set; }
        [ForeignKey(nameof(Product.Id))]
        public int ProductId { get; set; }
        public Product? Product { get; set; } // mot detail chi co mot product

        [ForeignKey(nameof(Cart.Id))]
        public int CartId { get; set; }      

        public Cart? Cart { get; set; } // mot detail chi co o 1 cart



    }
}
