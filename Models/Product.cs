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

        [ForeignKey(nameof(CategoryId))] // khoa ngoai
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
