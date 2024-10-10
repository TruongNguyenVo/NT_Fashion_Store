using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class OrderProductDetail
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Số lượng sản phẩm là bắt buộc.")]
        [Range(0, Int64.MaxValue, ErrorMessage = "Số lượng sản phẩm không được nhỏ hơn 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Gía sản phẩm là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm không được nhỏ hơn 0.")]
        public double PriceSale { get; set; }
        [ForeignKey(nameof(Product.Id))] // khoa ngoai lien ket voi bang product
        public int ProductId { get; set; }

        [ForeignKey(nameof(Order.Id))] // khoa ngoai lien ket voi bang Order
        public int OrderId { get; set; }

        public Product? Product { get; set; } // 1 orderdetail chi co 1 product
        public Order? Order { get; set; } //1 detail chi thuoc 1 order

    }
}
