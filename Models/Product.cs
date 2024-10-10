using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Color { get; set; }
        public string Dimension { get; set; }
        public string Material { get; set; }
        public string Quantity { get; set; }
        public double Price {  get; set; }
        public string Productor { get; set; }

        [ForeignKey(nameof(Category.Id))] // khoa ngoai voi category
        public int CategoryId { get; set; }
        public Category? Category { get; set; } // mot product chi co 1 category
        public List<CartDetail>? CartDetails { get; set; } // mot product co the nam trong nhieu cartdetail
        public List<ProductImage>? ProductImages { get; set; } // 1 product chua nhieu image
        public List<OrderProductDetail> orderProductDetails { get; set; } //1 product nam trong nhieu orderproductdetail
        public List<PurchaseReportProductDetail>? PurchaseReportProducts { get; set; } //1 product nam trong nhieu detail


    }
}
