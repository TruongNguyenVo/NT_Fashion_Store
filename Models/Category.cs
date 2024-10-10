using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId{  get; set;}
        public string? Description { get; set; }

        [ForeignKey(nameof(UserId))] // khoa ngoai lien ket voi id cua bang User
        public int? UserId{ get; set; }
        public User? User { get; set; }

        public List<Product>? Product { get; set; }

        
    }
}
