using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Màu sản phẩm là bắt buộc.")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Kích thước sản phẩm là bắt buộc.")]
        public string Dimension { get; set; }

        [Required(ErrorMessage = "Chất liệu là bắt buộc.")]
        public string Material { get; set; }

        [Required(ErrorMessage = "Số lượng sản phẩm là bắt buộc.")]
        [Range(0,Int64.MaxValue, ErrorMessage ="Số lượng sản phẩm không được nhỏ hơn 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Gía sản phẩm là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm không được nhỏ hơn 0.")]
        public double Price {  get; set; }

        [Required(ErrorMessage = "Nhà sản xuất là bắt buộc.")]
        public string Productor { get; set; }
        public Boolean IsDel { get; set; } = false;

        [ForeignKey(nameof(Category.Id))] // khoa ngoai voi category
        public int? CategoryId { get; set; }
        public Category? Category { get; set; } // mot product chi co 1 category
        public List<CartDetail>? CartDetails { get; set; } // mot product co the nam trong nhieu cartdetail
        public List<ProductImage>? ProductImages { get; set; } // 1 product chua nhieu image
        public List<OrderProductDetail>? orderProductDetails { get; set; } //1 product nam trong nhieu orderproductdetail
        public List<PurchaseReportProductDetail>? PurchaseReportProducts { get; set; } //1 product nam trong nhieu detail

    }

}
