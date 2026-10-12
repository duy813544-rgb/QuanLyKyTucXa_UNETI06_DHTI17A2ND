using System.ComponentModel.DataAnnotations;
namespace DoAnNhom06.Models;
/// <summary>Một dòng phòng kèm số người đang ở (tính bằng LINQ từ PhanPhong). SoChoCon là giá trị tính, không lưu.</summary>
public class PhongRow
{
    public int MaPhong { get; set; }
    public string TenPhong { get; set; } = "";
    public string ToaNha { get; set; } = "";
    public int Tang { get; set; }
    public string GioiTinhPhong { get; set; } = "";
    public string TrangThai { get; set; } = "";
    public string? MoTa { get; set; }
    public int MaLoaiPhong { get; set; }
    public string TenLoaiPhong { get; set; } = "";
    public decimal DonGiaThang { get; set; }
    public int SucChua { get; set; }
    public int SoNguoiDangO { get; set; }

    /// <summary>SoChoCon = SoNguoiToiDa - SoNguoiDangO. Không cho nhập tay.</summary>
    public int SoChoCon => SucChua - SoNguoiDangO;
}

/// <summary>Trạng thái truy vấn danh sách phòng. Giữ nguyên khi chuyển trang.</summary>
public class PhongDanhSachViewModel
{
    public List<PhongRow> Items { get; set; } = [];
    public string? Q { get; set; }
    public string? ToaNha { get; set; }
    public int? Tang { get; set; }
    public string? GioiTinhPhong { get; set; }
    public string? TrangThai { get; set; }
    public int? MaLoaiPhong { get; set; }
    /// <summary>"con" = còn chỗ, "het" = hết chỗ, rỗng = tất cả.</summary>
    public string? ConCho { get; set; }
    public string Sort { get; set; } = "ten_asc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
    public bool LaAdmin { get; set; }
    public List<LoaiPhong> LoaiPhongs { get; set; } = [];
    public List<string> ToaNhas { get; set; } = [];

    /// <summary>Bộ tham số truy vấn hiện tại, đổi số trang. Dùng cho liên kết phân trang.</summary>
    public Dictionary<string, string> RouteFor(int page)
    {
        var d = new Dictionary<string, string>();
        void Them(string khoa, string? giaTri) { if (!string.IsNullOrEmpty(giaTri)) d[khoa] = giaTri; }
        Them("q", Q);
        Them("toa", ToaNha);
        if (Tang is not null) d["tang"] = Tang.Value.ToString();
        Them("gioitinh", GioiTinhPhong);
        Them("trangthai", TrangThai);
        if (MaLoaiPhong is not null) d["loai"] = MaLoaiPhong.Value.ToString();
        Them("concho", ConCho);
        Them("sort", Sort);
        d["page"] = page.ToString();
        return d;
    }
}

public class TaiKhoanEditViewModel
{
    public int? MaTaiKhoan { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Tên đăng nhập từ 4 đến 50 ký tự.")]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = "";

    [Required(ErrorMessage = "Họ tên là bắt buộc.")]
    [StringLength(100)]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = "";

    [Required(ErrorMessage = "Email là bắt buộc.")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
    [StringLength(100)]
    public string Email { get; set; } = "";

    /// <summary>Bắt buộc khi tạo mới; khi sửa để trống nghĩa là giữ nguyên.</summary>
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string? MatKhau { get; set; }

    [Required]
    public string TrangThai { get; set; } = "DangHoatDong";
}

public class SinhVienEditViewModel
{
    public bool LaTaoMoi { get; set; }

    [Required(ErrorMessage = "Mã sinh viên là bắt buộc.")]
    [StringLength(20, ErrorMessage = "Mã sinh viên tối đa 20 ký tự.")]
    [Display(Name = "Mã sinh viên")]
    public string MaSinhVien { get; set; } = "";

    [Required(ErrorMessage = "Họ tên là bắt buộc.")]
    [StringLength(100)]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = "";

    [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày sinh")]
    public DateOnly? NgaySinh { get; set; }

    [Required(ErrorMessage = "Chọn giới tính.")]
    [Display(Name = "Giới tính")]
    public string GioiTinh { get; set; } = "Nam";

    [Required(ErrorMessage = "Lớp là bắt buộc.")]
    [StringLength(50)]
    public string Lop { get; set; } = "";

    [Required(ErrorMessage = "Khoa là bắt buộc.")]
    [StringLength(100)]
    public string Khoa { get; set; } = "";

    [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
    [RegularExpression(@"^\d{9,11}$", ErrorMessage = "Số điện thoại gồm 9 đến 11 chữ số.")]
    [Display(Name = "Số điện thoại")]
    public string SoDienThoai { get; set; } = "";

    [Required(ErrorMessage = "Email là bắt buộc.")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
    [StringLength(100)]
    public string Email { get; set; } = "";

    [StringLength(200)]
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }

    public string TrangThai { get; set; } = "DangHoatDong";

    // Chỉ dùng khi tạo mới: tài khoản đăng nhập của sinh viên.
    [StringLength(50)]
    [Display(Name = "Tên đăng nhập")]
    public string? TenDangNhap { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu ban đầu")]
    public string? MatKhau { get; set; }
}

public class DangKyKTXCreateViewModel
{
    [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu ở.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày bắt đầu ở")]
    public DateOnly? NgayBatDau { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn ngày kết thúc dự kiến.")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày kết thúc dự kiến")]
    public DateOnly? NgayKetThucDuKien { get; set; }

    [Display(Name = "Nguyện vọng loại phòng (tham khảo)")]
    public int? NguyenVongLoaiPhong { get; set; }

    [StringLength(300)]
    [DataType(DataType.MultilineText)]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }
}

public class PhanPhongChonViewModel
{
    public DangKyKTX DangKy { get; set; } = null!;
    public List<PhongRow> PhongPhuHop { get; set; } = [];
    public int? MaPhong { get; set; }
}

public class ChuyenPhongViewModel
{
    public PhanPhong HienTai { get; set; } = null!;
    public List<PhongRow> PhongPhuHop { get; set; } = [];
    public int? MaPhongMoi { get; set; }
}

public record SoPhongTheoLoai(string TenLoai, int SoPhong, int SucChua, int DangO);
public record PhongTheoSoNguoi(string TenPhong, string ToaNha, int SucChua, int DangO, int ConLai);
public record SinhVienTheoToa(string ToaNha, int SoSinhVien);
public record SinhVienTheoLoai(string TenLoai, int SoSinhVien);
public record DangKyTheoTrangThai(string TrangThai, int SoLuong);
public record VaoKtxTheoThang(int Nam, int Thang, int SoLuot);

public class ThongKeViewModel
{
    public List<SoPhongTheoLoai> SoPhongTheoLoai { get; set; } = [];
    public List<PhongTheoSoNguoi> PhongTheoSoNguoi { get; set; } = [];
    public List<SinhVienTheoToa> SinhVienTheoToa { get; set; } = [];
    public List<SinhVienTheoLoai> SinhVienTheoLoai { get; set; } = [];
    public List<DangKyTheoTrangThai> DangKyTheoTrangThai { get; set; } = [];
    public List<VaoKtxTheoThang> VaoKtxTheoThang { get; set; } = [];
    public PhongTheoSoNguoi? PhongDongNhat { get; set; }
    public int TongCho { get; set; }
    public int ChoDangSuDung { get; set; }
    public int ChoConLai { get; set; }
    /// <summary>Tỷ lệ = chỗ đang sử dụng / tổng chỗ x 100. Trả 0 khi tổng chỗ bằng 0.</summary>
    public double TyLeSuDung => TongCho == 0 ? 0 : Math.Round(ChoDangSuDung * 100.0 / TongCho, 1);
}

public class DashboardViewModel
{
    public int TongPhong { get; set; }
    public int TongSinhVien { get; set; }
    public int TongDangKy { get; set; }
    public int DangKyChoDuyet { get; set; }
    public int SinhVienDangO { get; set; }
    public int TongCho { get; set; }
    public int ChoDangSuDung { get; set; }
    public int ChoConTrong { get; set; }
    public int PhongDayDu { get; set; }
}

public class SinhVienHomeViewModel
{
    public SinhVien SinhVien { get; set; } = null!;
    public TaiKhoan TaiKhoan { get; set; } = null!;
    public DangKyKTX? DangKyChoDuyet { get; set; }
    public DangKyKTX? DangKyHienTai { get; set; }
    public PhanPhong? PhongHienTai { get; set; }
    public List<DangKyKTX> LichSuDangKy { get; set; } = [];

    /// <summary>Toàn bộ lịch sử ở thực tế (kể cả phòng cũ đã chuyển hoặc đã trả).</summary>
    public List<PhanPhong> LichSuPhong { get; set; } = [];
}

public class PhanPhongIndexViewModel
{
    public List<PhanPhong> DangO { get; set; } = [];
    public List<DangKyKTX> ChoDuyet { get; set; } = [];
    public string? Q { get; set; }
}
