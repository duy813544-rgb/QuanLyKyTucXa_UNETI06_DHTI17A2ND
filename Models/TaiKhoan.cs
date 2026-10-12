// Họ và tên: Vũ Lương Duy
// Mã sinh viên: 23203100014
using System.ComponentModel.DataAnnotations;

namespace DoAnNhom06.Models;

/// <summary>Entity TaiKhoan theo đề. Quan hệ 1-1 với SinhVien (vai trò SinhVien).</summary>
public partial class TaiKhoan
{
    [Key]
    [Display(Name = "Mã tài khoản")]
    public int MaTaiKhoan { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
    [StringLength(50, ErrorMessage = "Tên đăng nhập tối đa 50 ký tự.")]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = null!;

    [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu cần ít nhất 6 ký tự.")]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = null!;

    [Required(ErrorMessage = "Họ tên là bắt buộc.")]
    [StringLength(100)]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = null!;

    [Required(ErrorMessage = "Email là bắt buộc.")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    /// <summary>VaiTro: 'Admin' hoặc 'SinhVien' (xem <see cref="Models.VaiTro"/>).</summary>
    [Required]
    [StringLength(20)]
    public string VaiTro { get; set; } = "SinhVien";

    /// <summary>TrangThai: 'DangHoatDong' hoặc 'Khoa'.</summary>
    [Required]
    [StringLength(20)]
    public string TrangThai { get; set; } = "DangHoatDong";

    public virtual SinhVien? SinhVien { get; set; }
}
