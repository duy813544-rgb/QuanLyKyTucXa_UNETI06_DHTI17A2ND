using System.ComponentModel.DataAnnotations;

namespace DoAnNhom06.Models;


public partial class PhanPhong
{
    [Key]
    [Display(Name = "Mã phân phòng")]
    public int MaPhanPhong { get; set; }

    [Required]
    [Display(Name = "Mã đăng ký")]
    public int MaDangKy { get; set; }

    [Required]
    [Display(Name = "Mã phòng")]
    public int MaPhong { get; set; }

    [Required(ErrorMessage = "Ngày vào là bắt buộc.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày vào")]
    public DateOnly NgayVao { get; set; }

    /// <summary>Để trống khi còn đang ở; được cập nhật khi chuyển hoặc trả phòng.</summary>
    [DataType(DataType.Date)]
    [Display(Name = "Ngày ra")]
    public DateOnly? NgayRa { get; set; }

    [Required]
    [StringLength(20)]
    public string TrangThai { get; set; } = "DangO";

    public virtual DangKyKTX DangKyKTX { get; set; } = null!;

    public virtual PhongKTX PhongKTX { get; set; } = null!;
}
