// Họ và tên: Vũ Lương Duy
// Mã sinh viên: 23203100014
using System.ComponentModel.DataAnnotations;

namespace DoAnNhom06.Models;


public partial class DangKyKTX
{
    [Key]
    [Display(Name = "Mã đăng ký")]
    public int MaDangKy { get; set; }

    [Required]
    [StringLength(20)]
    [Display(Name = "Mã sinh viên")]
    public string MaSinhVien { get; set; } = null!;

    /// <summary>Do hệ thống xác định khi gửi đơn, không cho nhập tay.</summary>
    [DataType(DataType.Date)]
    [Display(Name = "Ngày đăng ký")]
    public DateOnly NgayDangKy { get; set; }

    [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày bắt đầu ở")]
    public DateOnly NgayBatDau { get; set; }

    [Required(ErrorMessage = "Ngày kết thúc dự kiến là bắt buộc.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày kết thúc dự kiến")]
    public DateOnly NgayKetThucDuKien { get; set; }

    [Display(Name = "Nguyện vọng loại phòng")]
    public int? NguyenVongLoaiPhong { get; set; }

    [StringLength(300)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    [Required]
    [StringLength(20)]
    public string TrangThai { get; set; } = "ChoDuyet";

    public virtual SinhVien SinhVien { get; set; } = null!;

    public virtual LoaiPhong? NguyenVong { get; set; }

    public virtual ICollection<PhanPhong> PhanPhongs { get; set; } = new List<PhanPhong>();
}
