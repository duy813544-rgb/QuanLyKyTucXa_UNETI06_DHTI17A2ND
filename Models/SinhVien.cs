// Họ và tên: Vũ Lương Duy
// Mã sinh viên: 23203100014
using System.ComponentModel.DataAnnotations;

namespace DoAnNhom06.Models;


public partial class SinhVien
{
    [Key]
    [Required(ErrorMessage = "Mã sinh viên là bắt buộc.")]
    [StringLength(20, ErrorMessage = "Mã sinh viên tối đa 20 ký tự.")]
    [Display(Name = "Mã sinh viên")]
    public string MaSinhVien { get; set; } = null!;

    [Required]
    [Display(Name = "Mã tài khoản")]
    public int MaTaiKhoan { get; set; }

    [Required(ErrorMessage = "Họ tên là bắt buộc.")]
    [StringLength(100)]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = null!;

    [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày sinh")]
    public DateOnly NgaySinh { get; set; }

    [Required(ErrorMessage = "Giới tính là bắt buộc.")]
    [StringLength(10)]
    [Display(Name = "Giới tính")]
    public string GioiTinh { get; set; } = null!;

    [Required(ErrorMessage = "Lớp là bắt buộc.")]
    [StringLength(50)]
    public string Lop { get; set; } = null!;

    [Required(ErrorMessage = "Khoa là bắt buộc.")]
    [StringLength(100)]
    public string Khoa { get; set; } = null!;

    [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
    [RegularExpression(@"^\d{9,11}$", ErrorMessage = "Số điện thoại gồm 9 đến 11 chữ số.")]
    [Display(Name = "Số điện thoại")]
    public string SoDienThoai { get; set; } = null!;

    [Required(ErrorMessage = "Email là bắt buộc.")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(200)]
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }

    [Required]
    [StringLength(20)]
    public string TrangThai { get; set; } = "DangHoatDong";

    public virtual TaiKhoan TaiKhoan { get; set; } = null!;

    public virtual ICollection<DangKyKTX> DangKyKTXs { get; set; } = new List<DangKyKTX>();
}
