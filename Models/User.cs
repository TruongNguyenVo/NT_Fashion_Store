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
    }
}
