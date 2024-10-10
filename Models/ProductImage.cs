using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Product.Id))] // khoa ngoai lien ket voi product
        public int ProductId {  get; set; }
        public string ImgURL { get; set; }

        public Product Product { get; set; } // mot product image thuoc 1 product
    }
}
