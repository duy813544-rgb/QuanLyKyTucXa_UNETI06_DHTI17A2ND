
using System.ComponentModel.DataAnnotations;

namespace DoAnNhom06.Models;
public sealed class DangKyTaiKhoanViewModel
{
    [Required(ErrorMessage = "Nhập mã sinh viên.")]
    [StringLength(20, ErrorMessage = "Mã sinh viên tối đa 20 ký tự.")]
    [Display(Name = "Mã sinh viên")]
    public string MaSinhVien { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập họ và tên.")]
    [StringLength(100)]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Chọn ngày sinh.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày sinh")]
    public DateOnly? NgaySinh { get; set; }

    [Required(ErrorMessage = "Chọn giới tính.")]
    [Display(Name = "Giới tính")]
    public string GioiTinh { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập lớp.")]
    [StringLength(50)]
    public string Lop { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập khoa.")]
    [StringLength(100)]
    public string Khoa { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập email.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập số điện thoại.")]
    [RegularExpression(@"^\d{9,11}$", ErrorMessage = "Số điện thoại gồm 9 đến 11 chữ số.")]
    [Display(Name = "Số điện thoại")]
    public string SoDienThoai { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập tên đăng nhập.")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Tên đăng nhập từ 4 đến 50 ký tự.")]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập mật khẩu.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nhập lại mật khẩu.")]
    [Compare(nameof(MatKhau), ErrorMessage = "Mật khẩu xác nhận không khớp.")]
    [DataType(DataType.Password)]
    [Display(Name = "Xác nhận mật khẩu")]
    public string XacNhanMatKhau { get; set; } = string.Empty;
}
