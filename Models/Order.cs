using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using doan1_v1.Helpers;

namespace doan1_v1.Models
{
    public class Order
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Ngày đặt hàng không được để trống")]
        public DateOnly DateOrder { get; set; }


        [DataType(DataType.Date)]
        [DateNotAfter(nameof(DateOrder), ErrorMessage ="Ngày nhận không được nhỏ hơn ngày đặt hàng")]
        public DateOnly? DateReceive { get; set; }

        [Required(ErrorMessage = "Chi phí vận chuyển là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Chi phí vận chuyển không được nhỏ hơn 0.")]
        public double DeliveryCost { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Các chi phí khác không được nhỏ hơn 0.")]
        public double? OtherCost { get; set; } = 0;

        [Required(ErrorMessage = "Trạng thái không được để trống.")]
        [DefaultValue("Đã đặt hàng")]
        [RegularExpression(@"^(Đã đặt hàng|Đang xử lý|Đang vận chuyển|Đã thanh toán|Đã hủy)$", ErrorMessage = "Chỉ nhận các giá trị Đã đặt hàng, Đang xử lý, Đang vận chuyển, Đã thanh toán, Đã hủy.")]
        public required string Status { get; set; }

        public string? Note { get; set; }

        public Boolean IsDel { get; set; } = false;

        [ForeignKey(nameof(User.Id))] // khoa ngoai lien ket voi bang User
        public int? AdminId { get; set; }

        [ForeignKey(nameof(User.Id))] // khoa ngoai lien ket voi bang User
        public int? CustomerId { get; set; }
        public User? Customer { get; set; } // order chi co 1 khach hang

        public List<OrderProductDetail>? OrderProductDetails { get; set; } // mot order co nhieu ordeDetail


    }
}
