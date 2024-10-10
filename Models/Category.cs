using doan1_v1.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace doan1_v1.Models
{
    public class Category
    {
        public int Id { get; set; }


        [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự.")]
        [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
        public string Name { get; set; }

        [ForeignKey(nameof(Category.Id))]
        public int? ParentId{get; set;}

        public string? Description { get; set; }

        [ForeignKey(nameof(UserId))] // khoa ngoai lien ket voi id cua bang User
        public int? UserId{ get; set; }
        public User? User { get; set; } // 1 category quan ly boi 1 user

        public List<Product>? Product { get; set; } // mot category co nhieu product

        
    }
}
