
using System.ComponentModel.DataAnnotations;

namespace DoAnNhom06.Models;

public partial class LoaiPhong
{
    [Key]
    [Display(Name = "Mã loại phòng")]
    public int MaLoaiPhong { get; set; }

    [Required(ErrorMessage = "Tên loại phòng là bắt buộc.")]
    [StringLength(100)]
    [Display(Name = "Tên loại phòng")]
    public string TenLoaiPhong { get; set; } = null!;

    [Range(1, 50, ErrorMessage = "Số người tối đa phải từ 1 đến 50.")]
    [Display(Name = "Số người tối đa")]
    public int SoNguoiToiDa { get; set; }

    [Range(1, 100000000, ErrorMessage = "Đơn giá tháng phải lớn hơn 0.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Đơn giá tháng (VNĐ)")]
    public decimal DonGiaThang { get; set; }

    [StringLength(250)]
    [DataType(DataType.MultilineText)]
    public string? MoTa { get; set; }

    [Required]
    [StringLength(20)]
    public string TrangThai { get; set; } = "DangSuDung";

    public virtual ICollection<PhongKTX> PhongKTXs { get; set; } = new List<PhongKTX>();
}
