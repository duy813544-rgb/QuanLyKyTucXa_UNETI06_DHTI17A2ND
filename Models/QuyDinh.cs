
namespace DoAnNhom06.Models;


public static class VaiTro
{
    public const string Admin = "Admin";
    public const string SinhVien = "SinhVien";
}

public static class TrangThaiChung
{
    public const string DangHoatDong = "DangHoatDong";
    public const string Khoa = "Khoa";
}

/// <summary>Trạng thái loại phòng.</summary>
public static class TrangThaiLoaiPhong
{
    public const string DangSuDung = "DangSuDung";
    public const string NgungSuDung = "NgungSuDung";
}

/// <summary>Trạng thái phòng KTX.</summary>
public static class TrangThaiPhong
{
    public const string DangSuDung = "DangSuDung";
    public const string TamNgung = "TamNgung";
}

/// <summary>Trạng thái đăng ký KTX: Chờ duyệt -> Đã phân phòng -> Đã trả phòng; có thể Đã hủy.</summary>
public static class TrangThaiDangKy
{
    public const string ChoDuyet = "ChoDuyet";
    public const string DaPhanPhong = "DaPhanPhong";
    public const string DaTraPhong = "DaTraPhong";
    public const string DaHuy = "DaHuy";
}

/// <summary>Trạng thái một lần ở phòng (bản ghi lịch sử phân phòng).</summary>
public static class TrangThaiPhanPhong
{
    public const string DangO = "DangO";
    public const string DaChuyen = "DaChuyen";
    public const string DaTraPhong = "DaTraPhong";
}

/// <summary>Giới tính. Dùng cho sinh viên và giới tính quy định của phòng.</summary>
public static class GioiTinh
{
    public const string Nam = "Nam";
    public const string Nu = "Nữ";
}

/// <summary>Nhãn tiếng Việt cho các giá trị lưu trong CSDL. Dùng trong View.</summary>
public static class Nhan
{
    // Tên phương thức có tiền tố "Nhan" để không trùng với tên các lớp hằng số bên trên.
    public static string NhanChung(string? v) => v switch
    {
        TrangThaiChung.DangHoatDong => "Đang hoạt động",
        TrangThaiChung.Khoa => "Bị khóa",
        _ => v ?? ""
    };

    public static string NhanLoaiPhong(string? v) => v switch
    {
        TrangThaiLoaiPhong.DangSuDung => "Đang sử dụng",
        TrangThaiLoaiPhong.NgungSuDung => "Ngừng sử dụng",
        _ => v ?? ""
    };

    public static string NhanPhong(string? v) => v switch
    {
        TrangThaiPhong.DangSuDung => "Đang sử dụng",
        TrangThaiPhong.TamNgung => "Tạm ngừng",
        _ => v ?? ""
    };

    public static string NhanDangKy(string? v) => v switch
    {
        TrangThaiDangKy.ChoDuyet => "Chờ duyệt",
        TrangThaiDangKy.DaPhanPhong => "Đã phân phòng",
        TrangThaiDangKy.DaTraPhong => "Đã trả phòng",
        TrangThaiDangKy.DaHuy => "Đã hủy",
        _ => v ?? ""
    };

    public static string NhanPhanPhong(string? v) => v switch
    {
        TrangThaiPhanPhong.DangO => "Đang ở",
        TrangThaiPhanPhong.DaChuyen => "Đã chuyển",
        TrangThaiPhanPhong.DaTraPhong => "Đã trả phòng",
        _ => v ?? ""
    };

    public static string NhanVaiTro(string? v) => v switch
    {
        VaiTro.Admin => "Quản trị viên",
        VaiTro.SinhVien => "Sinh viên",
        _ => v ?? ""
    };
}
